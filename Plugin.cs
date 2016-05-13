using KeePass.Plugins;

namespace KeePassUnicodeGen
{
    public class KeePassUnicodeGenExt : Plugin {
        private IPluginHost PluginHost;
        private Generator Gen;

        public override bool Initialize(IPluginHost Host) {
            PluginHost = Host;
            Gen = new Generator();
            PluginHost.PwGeneratorPool.Add(Gen);
            return true;
        }

        public override void Terminate() {
            if (PluginHost != null) {
                PluginHost.PwGeneratorPool.Remove(Gen.Uuid);
                Gen = null;
                PluginHost = null;
            }
        }
    }
}
