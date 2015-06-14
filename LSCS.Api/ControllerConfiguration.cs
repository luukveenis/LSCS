using System;
using System.Web.Configuration;

namespace LSCS.Api
{
    public class ControllerConfiguration : IControllerConfiguration
    {
        private static int? _pageSizeLimit;

        public int PageSizeLimit
        {
            get
            {
                if (!_pageSizeLimit.HasValue)
                {
                    _pageSizeLimit = Convert.ToInt32(WebConfigurationManager.AppSettings["apiMaxPageSize"]);
                }
                return _pageSizeLimit.Value;
            }
        }
    }
}