using System;

namespace UI.Config
{
    public class VkConfig
    {
        public string Token => Environment.GetEnvironmentVariable("VK_TOKEN");

        public string GroupId => Environment.GetEnvironmentVariable("VK_GROUP_ID");
    }
}