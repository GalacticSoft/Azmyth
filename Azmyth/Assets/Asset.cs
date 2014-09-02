using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Assets
{
    public abstract class Asset : IHasRect
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

        public virtual void AddAsset(Asset asset)
        {
            _assets.Add(asset.AssetID, asset);
        }

        public virtual void RemoveAsset(VectorID assetID)
        {
            _assets.Remove(assetID);
        }

        public virtual void RemoveAsset(Asset asset)
        {
            RemoveAsset(asset.AssetID);
        }

        public virtual void RemoveAssets(long vector)
        {
            if (_assets[vector] != null)
            {
                _assets[vector].Clear();
            }
        }

        public virtual void RemoveAssets()
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

        private System.Drawing.RectangleF m_bounds = new System.Drawing.RectangleF();

        public System.Drawing.RectangleF Bounds
        {
            get { return m_bounds; }
            set 
            {
                m_bounds = value;

                EventHandler boundsChanged = BoundsChanged;

                if(boundsChanged != null)
                {
                    boundsChanged.BeginInvoke(this, new EventArgs(), null, null);
                }
            }
        }

        public event EventHandler BoundsChanged;
    }
}
