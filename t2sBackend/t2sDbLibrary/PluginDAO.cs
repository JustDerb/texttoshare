using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t2sDbLibrary
{
    public class PluginDAO
    {
        public int? PluginID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool IsDisabled
        {
            get;
            set;
        }

        public string VersionNum
        {
            get;
            set;
        }

        public int? OwnerID
        {
            get;
            set;
        }

        public PluginAccess Access
        {
            get;
            set;
        }

        public string HelpText
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PluginDAO p = obj as PluginDAO;

            return (
                this.PluginID == p.PluginID &&
                this.Name.Equals(p.Name) &&
                this.Description.Equals(p.Description) &&
                this.IsDisabled == p.IsDisabled &&
                this.VersionNum.Equals(p.VersionNum) &&
                this.OwnerID == p.OwnerID &&
                this.Access.Equals(p.Access) &&
                this.HelpText.Equals(p.HelpText)
            );
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + (null == PluginID ? 0 : PluginID.GetHashCode());
                hash = hash * 23 + (null == Name ? 0 : Name.GetHashCode());
                hash = hash * 23 + (null == Description ? 0 : Description.GetHashCode());
                hash = hash * 23 + IsDisabled.GetHashCode();
                hash = hash * 23 + (null == VersionNum ? 0 : VersionNum.GetHashCode());
                hash = hash * 23 + OwnerID.GetHashCode();
                hash = hash * 23 + Access.GetHashCode();
                hash = hash * 23 + (null == HelpText ? 0 : HelpText.GetHashCode());

                return hash;
            }
        }
    }
}
