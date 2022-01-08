using System.Text;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class CloudApparatusProvider : ABlobContainerBasedProvider<SrApparatus>, IApparatusProvider
    {
        public CloudApparatusProvider(string containerURL) : base(containerURL + "/apparatus") { }
        
        protected override SrApparatus Convert(byte[] bytes)
        {
            string text = Encoding.UTF8.GetString(bytes);
            return JsonUtility.FromJson<SrApparatus>(text);        
        }
    }
}

