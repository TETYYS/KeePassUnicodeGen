using System.Linq;
using System.Text;

using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;

namespace KeePassUnicodeGen {
    class Generator : CustomPwGenerator {
        public override ProtectedString Generate(PwProfile prf, CryptoRandomStream crsRandomSource) {
            byte[] str = new byte[prf.Length * 2];

            if (prf.Length > 0xD7FF)
                prf.Length = 0xD7FF - 1;

            for (int i = 0; i < prf.Length * 2; i+=2) {
                byte[] chr = crsRandomSource.GetRandomBytes(2);
                bool invalid = false;

                if (prf.NoRepeatingCharacters) {
                    for (int x = 0; x < prf.Length * 2; x += 2) {
                        if (str[x] == chr[0] && str[x+1] == chr[1]) {
                            invalid = true;
                            break;
                        }
                    }
                }

                if (prf.ExcludeLookAlike && "1Ii!|0Z2S5oOl".Contains(Encoding.Unicode.GetChars(new[] { chr[0], chr[1]})[0]))
                    invalid = true;

                if (prf.ExcludeCharacters.Contains(Encoding.Unicode.GetChars(new[] {chr[0], chr[1]})[0]))
                    invalid = true;

                if (invalid) {
                    i -= 2;
                    continue;
                }

                str[i] = chr[0];
                str[i+1] = chr[1];
            }

            return new ProtectedString(true, Encoding.Unicode.GetString(str));
        }

        public override PwUuid Uuid => new PwUuid(new byte[] { 0xe8, 0x16, 0xe7, 0xb0, 0x23, 0x4d, 0x42, 0x7f, 0x96, 0x9e, 0x2d, 0xc7, 0x22, 0xb3, 0x2, 0xa9 });

        public override string Name => "Random unicode";
    }
}
