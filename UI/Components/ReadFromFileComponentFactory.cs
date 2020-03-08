using LiveSplit.Model;
using System;

namespace LiveSplit.UI.Components
{
    public class ReadFromFileComponentFactory : IComponentFactory
    {
        public string ComponentName => "Read Text From File";

        public string Description => "Displays any text from a file.";

        public ComponentCategory Category => ComponentCategory.Information;

        public IComponent Create(LiveSplitState state) => new ReadFromFileComponent(state);

        public string UpdateName => ComponentName;

        public string XMLURL => "";

        public string UpdateURL => "";

        public Version Version => Version.Parse("1.0.0");
    }
}
