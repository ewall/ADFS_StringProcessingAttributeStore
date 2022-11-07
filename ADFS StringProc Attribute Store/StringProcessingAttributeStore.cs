﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityServer.ClaimsPolicy.Engine.AttributeStore;
using System.IdentityModel;

/* This ADFS Custom Attribute Store would allow us to transform claim rules with basic string manipulation, such as changing to all lowercase or all caps.
 *
 * The code started straight from Microsoft's example here:
 *    https://msdn.microsoft.com/en-us/library/hh599320.aspx
 * ...with assistance from this article:
 *    https://learn.microsoft.com/en-us/archive/blogs/cloudpfe/how-to-create-a-custom-attribute-store-for-active-directory-federation-services-3-0
 * See those pages for details on compiling, installing, and using the DLL.
 * 
 * --Eric Wallace, March 2015
 */

namespace StringProcessingAttributeStore
{
    public class StringProcessingAttributeStore :IAttributeStore
    {
        #region IAttributeStore Members

        public IAsyncResult BeginExecuteQuery(string query, string[] parameters, AsyncCallback callback, object state)
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new AttributeStoreQueryFormatException("No query string.");
            }

            if (parameters == null)
            {
                throw new AttributeStoreQueryFormatException("No query parameter.");
            }

            if (parameters.Length != 1)
            {
                throw new AttributeStoreQueryFormatException("More than one query parameter.");
            }

            string inputString = parameters[0];

            if (inputString == null)
            {
                throw new AttributeStoreQueryFormatException("Query parameter cannot be null.");
            }

            string result = null;

            switch (query)
            {
                case "toUpper":
                    {
                        result = inputString.ToUpper();
                        break;
                    }
                case "toLower":
                    {
                        result = inputString.ToLower();
                        break;
                    }
                case "base64":
                    {
                        // MS says we can assume that input string is in UTF-8 form?
                        result = Convert.ToBase64String(Encoding.UTF8.GetBytes(inputString));
                        break;
                    }
                case "trim":
                    {
                        result = inputString.Trim();
                        break;
                    }
                case "removeDashes":
                    {
                        result = inputString.Replace("-", "");
                        break;
                    }
                case "truncateTo12Chars":
                    {
                        result = Truncate(inputString, 12);
                        break;
                    }
                case "truncateTo16Chars":
                    {
                        result = Truncate(inputString, 16);
                        break;
                    }
                default:
                    {
                        throw new AttributeStoreQueryFormatException(String.Format("The query string \"{0}\" is not supported.", query));
                    }
            }
            string[][] outputValues = new string[1][];
            outputValues[0] = new string[1];
            outputValues[0][0] = result;

            TypedAsyncResult<string[][]> asyncResult = new TypedAsyncResult<string[][]>(callback, state);
            asyncResult.Complete(outputValues, true);
            return asyncResult;
        }

        public string[][] EndExecuteQuery(IAsyncResult result)
        {
            return TypedAsyncResult<string[][]>.End(result);
        }

        public void Initialize(Dictionary<string, string> config)
        {
            // No initialization is required for this store.
        }

        #endregion

        private static string Truncate(string value, int maxLength)
        {
            if (String.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
