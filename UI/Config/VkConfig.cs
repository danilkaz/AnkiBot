using System;

namespace UI.Config
{
    public class VkConfig
    {
        public string Token => Environment.GetEnvironmentVariable("VK_TOKEN", EnvironmentVariableTarget.User);

        public string GroupId => Environment.GetEnvironmentVariable("VK_GROUP_ID", EnvironmentVariableTarget.User);
        
    }
}