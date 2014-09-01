using System;
using System.Collections.Generic;

namespace Azmyth.Assets
{
    public class AssetStore
    {
        protected long _maxIndex;
        protected long _maxVector;
        protected List<VectorID> _vectorIDList;
        protected VectorList<Asset> _assetList;
        protected Dictionary<long, Type> _typeList;
        protected Dictionary<Type, long> _typeIndexList;
        protected Dictionary<long, long> _currentIDList;
        
        public AssetStore()
        {
            _maxIndex = 1;
            _maxVector = 1;
            _assetList = new VectorList<Asset>();
            _typeList = new Dictionary<long, Type>();
            _typeIndexList= new Dictionary<Type,long>();
            _currentIDList = new Dictionary<long, long>();
            _vectorIDList = new List<VectorID>();
        }


        public long AddAssetType(Type assetType)
        {
            if (!_typeIndexList.ContainsKey(assetType))
            {
                if (!_currentIDList.ContainsKey(_maxVector))
                {
                    _typeList.Add(_maxVector, assetType);
                    _typeIndexList.Add(assetType, _maxVector);
                    _currentIDList.Add(_maxVector, 1);
                    _maxVector++;
                }
            }

            return _maxVector;
        }

        public virtual VectorID CreateAsset(long vector)
        {
            VectorID assetID = null;

            if (_typeList.ContainsKey(vector))
            {
                assetID = CreateAsset(_typeList[vector]);
            }

            return assetID;
        }

        public virtual VectorID CreateAsset(Type assetType)
        {   
            long vectorID;
            long assetID;
            Asset asset;

            asset = null;

            VectorID id = null;
           
            if(assetType.IsSubclassOf(typeof(Asset)))
            {
                if(!_typeIndexList.ContainsKey(assetType))
                {
                    _typeIndexList.Add(assetType, _maxIndex);

                    while (_currentIDList.ContainsKey(_maxIndex))
                    {
                        _maxIndex++;
                    }
                }

                vectorID = _typeIndexList[assetType];

                if(!_currentIDList.ContainsKey(vectorID))
                {
                    _currentIDList.Add(vectorID, 1);
                }

                try
                {
                     assetID = _currentIDList[vectorID];
                    id = new VectorID(vectorID, assetID);
                    _currentIDList[vectorID]++;
                    asset = Activator.CreateInstance(assetType, id) as Asset;
                    _assetList.Add(id, asset);
                }
                catch
                {
                    _currentIDList[vectorID]--;
                    throw new InvalidCastException("unable to create inststance of " + assetType.FullName);
                }
        
            }
            else
            {
                throw new InvalidCastException("entityType does not derive from Entity.");
            }

            return id;
        }

        public virtual void AddAsset(Asset asset)
        {
            if(!_assetList.ContainsKey(asset.AssetID))
            {
                _assetList.Add(asset.AssetID, asset);
            }
        }

        public virtual void RemoveAsset(VectorID id)
        {
            if (_assetList.ContainsKey(id))
            {
                _assetList.Remove(id);
            }
        }

        public Asset this[VectorID vectorID]
        {
            get
            {
                Asset entity = null;

                if(_assetList.ContainsKey(vectorID))
                {
                    entity = _assetList[vectorID];
                }

                return entity;
            }
        }
    }
}
