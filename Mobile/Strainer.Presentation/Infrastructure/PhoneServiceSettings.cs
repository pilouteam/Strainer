using System;
using System.IO;
using Strainer.Infrastructure;
using Cirrious.CrossCore;
using System.Threading.Tasks;

namespace Strainer.Infrastructure
{

	public class PhoneServiceSettings
	{
        public PhoneServiceSettings()
        {
            EmailAddress = "mears@apcurium.com";
        }
        public string EmailAddress
        {
            get;
            set;
        }
	}

}

