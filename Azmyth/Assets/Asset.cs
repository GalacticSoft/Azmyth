using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Assets
{
    public abstract class Asset
    {
        public string Name { get; set; }

        public VectorID AssetID { get; set; }

        //protected VectorList<Affect> _affects;
        protected VectorList<Asset> _assets;

        public Asset(VectorID vectorID)
        {
            AssetID = vectorID;

            _assets = new VectorList<Asset>();

        }

        protected Asset()
            : this(new VectorID(-1, -1))
        {

        }


        //public virtual void ApplyAffect(Affect affect)
        //{
        //    if(!_affects.ContainsKey(affect.AffectID))
        //    {
        //        _affects.Add(affect.AffectID, affect);

        //        foreach (VectorID statID in _stats.Keys)
        //        {
        //            if (affect[statID] != null)
        //            {
        //                _stats[statID].Modifier += affect[statID].Modifier;
        //            }
        //        }
        //    }
        //}

        //public virtual void RemoveAffect(Affect affect)
        //{
        //    if (_affects.ContainsKey(affect.AffectID))
        //    {
        //        _affects.Remove(affect.AffectID);

        //        foreach (VectorID statID in _stats.Keys)
        //        {
        //            if (affect[statID] != null)
        //            {
        //                _stats[statID].Modifier -= affect[statID].Modifier;
        //            }
        //        }
        //    }
        //}

        //public virtual void RemoveAffects()
        //{
        //    _affects.Clear();
        //}

        //public virtual void RemoveAffects(long vector)
        //{
        //    if (_affects.Vectors.Contains(vector))
        //    {
        //        foreach (VectorID statID in _affects[vector].Keys)
        //        {
        //            // RemoveAffect(_affects[statID]);
        //        }
        //    }
        //}

        //public virtual void UpdateAffects()
        //{
        //    foreach (Affect affect in _affects.Values)
        //    {
        //        if (affect != null)
        //        {
        //            affect.Duration--;

        //            if (affect.Duration == 0)
        //            {
        //                //RemoveAffect(affect);
        //            }
        //        }
        //    }
        //}

        public virtual void AddObject(Asset obj)
        {
            _assets.Add(obj.AssetID, obj);
        }

        public virtual void RemoveObject(VectorID objID)
        {
            _assets.Remove(objID);
        }

        public virtual void RemoveObject(Asset obj)
        {
            RemoveObject(obj.AssetID);
        }

        public virtual void RemoveObjects(long vector)
        {
            if (_assets[vector] != null)
            {
                _assets[vector].Clear();
            }
        }

        public virtual void RemoveObjects()
        {
            _assets.Clear();
        }

        public virtual Dictionary<VectorID, Asset> this[long vector]
        {
            get
            {
                return _assets[vector];
            }
        }

        public virtual Asset this[long vector, long id]
        {
            get
            {
                return _assets[vector, id];
            }
        }

        public virtual Asset this[VectorID id]
        {
            get
            {
                return _assets[id];
            }
            set
            {
                _assets[id] = value;
            }
        }
    }
}
