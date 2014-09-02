using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Azmyth
{
	public enum VectorTypes
    {
        Bit             = 1,
        Value           = 2,
    };

    [Serializable]
    [XmlRoot("VectorID")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class VectorID : IComparable, IComparable<VectorID>, IEquatable<VectorID>
    {   
        [XmlElement("ID")]
        public long ID { get; set; }    

        [XmlElement("Vector")]
        public long Vector { get; set; }

        [XmlIgnore]
        public VectorTypes VectorType { get; set; }

        public VectorID() : this(-1, -1, VectorTypes.Value) { }

        public VectorID(long vector, long id) : this(vector, id, VectorTypes.Value) { }

        public VectorID(long vector, long id, VectorTypes vectorType)
        {
            ID = id;
            Vector = vector;
            VectorType = vectorType;
        }

        public static bool operator==(VectorID id1, VectorID id2)
        {

            if (object.Equals(id1, null))
            {
                return false;
            }

            return id1.Equals(id2);
        }

        public static bool operator !=(VectorID id1, VectorID id2)
        {
            if (id2 == null)
                return false;

            if (id1 == null)
                return false;

            return !id1.Equals(id2);
        }

        public static bool operator <(VectorID id1, VectorID id2)
        {
            if (id1.Equals(id2))
            {
                return false;
            }

            if (id1.Vector < id2.Vector)
            {
                return true;
            }

            if (id1.Vector <= id2.Vector && id1.ID < id2.ID)
            {
                return true;
            }

            return false;
        }

        public static bool operator <=(VectorID id1, VectorID id2)
        {
            if (id1.Equals(id2))
            {
                return true;
            }

            if (id1.Vector < id2.Vector)
            {
                return true;
            }

            if (id1.Vector <= id2.Vector && id1.ID <= id2.ID)
            {
                return true;
            }

            return false;
        }

        public static bool operator >(VectorID id1, VectorID id2)
        {
            if (id1.Equals(id2))
            {
                return false;
            }

            if (id1.Vector > id2.Vector)
            {
                return true;
            }

            if (id1.Vector >= id2.Vector && id1.ID > id2.ID)
            {
                return true;
            }

            return false;
        }

        public static bool operator >=(VectorID id1, VectorID id2)
        {
            if (id1.Equals(id2))
            {
                return true;
            }

            if (id1.Vector > id2.Vector)
            {
                return true;
            }

            if (id1.Vector >= id2.Vector && id1.ID >= id2.ID)
            {
                return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            return this.Vector == ((VectorID)obj).Vector && this.ID == ((VectorID)obj).ID;
        }

        public override int GetHashCode()
        {
            return (int)Vector & ((int)ID ^ (int)Vector);
        }

        public override string ToString()
        {
            return Vector + ": " + ID;
        }

        #region IEquatable<VectorID> Members

        public bool Equals(VectorID other)
        {
            if (object.Equals(other, null))
            {
                return false;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            
            return this.Vector == other.Vector && this.ID == other.ID;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            VectorID id = (VectorID)obj;

            if (this > id)
            {
                return 1;
            }

            if (this < id)
            {
                return -1;
            }

            return 0;
        }

        #endregion

        #region IComparable<VectorID> Members

        public int CompareTo(VectorID other)
        {
            if (this > other)
            {
                return 1;
            }

            if (this < other)
            {
                return -1;
            }

            return 0;
        }

        #endregion
    }
}
