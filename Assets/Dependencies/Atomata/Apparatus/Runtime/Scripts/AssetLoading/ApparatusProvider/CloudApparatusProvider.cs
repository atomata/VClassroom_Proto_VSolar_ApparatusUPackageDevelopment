using System.Text;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class CloudApparatusProvider : ABlobContainerBasedProvider<SrNode>, IApparatusProvider
    {
        public CloudApparatusProvider(string containerURL) : base(containerURL) { }
        
        protected override SrNode Convert(byte[] bytes)
        {
            string text = Encoding.UTF8.GetString(bytes);
            return JsonUtility.FromJson<SrNode>(text);        
        }
    }
}

