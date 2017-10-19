using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

namespace BB.Implementation.Config
{
    public class SecuritySection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public PasswordPolicies PasswordPolicies
        {
            get { return (PasswordPolicies)this[""]; }
            set { this[""] = value; }
        }
    }

    public class PasswordPolicies: ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PasswordPolicy();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //set to whatever Element Property you want to use for a key
            return ((PasswordPolicy)element).key;
        }

        public new PasswordPolicy this[string key]
        {
            get
            {
                return this.OfType<PasswordPolicy>().FirstOrDefault(item => item.key == key);
            }
        }
    }

    public class PasswordPolicy : ConfigurationElement
    {
        public override bool IsReadOnly()
        {
            return false;
        }

        //Make sure to set IsKey=true for property exposed as the GetElementKey above
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string key
        {
            get { return (string)base["key"]; }
            set { base["key"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string value
        {
            get { return (string)base["value"]; }
            set { base["value"] = value; }
        }

        [ConfigurationProperty("Message", IsRequired = false)]
        public string Message
        {
            get { return (string)base["Message"]; }
            set { base["Message"] = value; }
        }

        [ConfigurationProperty("Message", IsRequired = false)]
        public string FormattedMessage
        {
            get { return String.Format((string)base["Message"], (string)base["value"]); }
            
        }

    }
}
