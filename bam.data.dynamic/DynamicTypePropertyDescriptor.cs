using Bam.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Data.Dynamic;

namespace Bam.Data.Dynamic.Data
{
    [Serializable]
    public class DynamicTypePropertyDescriptor: RepoData
    {
        public ulong DynamicTypeDescriptorId { get; set; }
        public virtual DynamicTypeDescriptor? ParentType { get; set; }
        public string? ParentTypeName { get; init; }
        public string? PropertyType { get; init;}
        public string? PropertyName { get; init; }

        public override int GetHashCode()
        {
            return $"{ParentTypeName}:{PropertyType ?? "object"}:{PropertyName}".ToSha256Int();
        }

        public override bool Equals(object obj)
        {
            if(obj is DynamicTypePropertyDescriptor d)
            {
                return d.GetHashCode() == GetHashCode();
            }
            return false;
        }
    }
}
