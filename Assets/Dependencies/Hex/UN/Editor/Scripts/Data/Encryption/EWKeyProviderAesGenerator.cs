using System.Security.Cryptography;
using System.Text;
using HexCS.Data.Generation.CSharp;
using HexUN.EditorElements;
using UnityEditor;

namespace HexUN.Data
{
    public class EWKeyProviderAesGenerator : EditorWindow
    {
        private const string cDefaultFileExtension = "cs";

        private StringBuilder _sb;
        private AesManaged _aes; 

        private EESaveFileButton _saveFileButton;
        private string _namespace = "HexUN.Generated";
        private EECustomString _fileName;


        private void OnEnable()
        {
            _sb = new StringBuilder();
            _aes = new AesManaged();

            _saveFileButton = new EESaveFileButton(
                new SLabel()
                {
                    Id = "save-key-provider",
                    ReadableName = "Save KeyProvider",
                    ToolTip = "Click to choose a path to save the genrated key provider"
                }
            );

            _saveFileButton.OnPathChosen += GenerateKeyProvider;

            _fileName = new EECustomString(
                new SLabel()
                {
                    Id = "filename",
                    ReadableName = "FileName",
                    ToolTip = "This is the name of the generated KeyProviderAesSource"
                },
                () => $"{_namespace.Split('.')[0]}KeyProviderAesSource"
            );
        }

        // Add menu named "My Window" to the Window menu
        [MenuItem("Hex/Encryption/AesKeyProviderGenerator")]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            EWKeyProviderAesGenerator window = (EWKeyProviderAesGenerator)EditorWindow.GetWindow(typeof(EWKeyProviderAesGenerator));
            window.Show();
        }

        void OnGUI()
        {
            _namespace = EditorGUILayout.TextField("Namespace", _namespace);
            _fileName.Render_Basic();
            _saveFileButton.Render_Basic(_fileName.String, cDefaultFileExtension);
        }

        private void GenerateKeyProvider(UnityPath p)
        {
            _aes.GenerateKey();
            _aes.GenerateIV();

            _sb.Clear();

            using (GTFile f = new GTFile(_sb, p, Encoding.UTF8))
            {
                using (GTUsings u = f.Generate_Usings<GTUsings>())
                {
                    u.SetRequired("HexCS.Encryption");
                }

                using (GTNamespace n = f.Generate_Namespace<GTNamespace>())
                {
                    n.SetRequired(_namespace);

                    using (GTClass cls = n.Generate_NamespaceObject<GTClass>())
                    {
                        cls.SetRequired(_fileName.String, EKeyword.PUBLIC);
                        cls.Add_Inheritances("AKeyProviderAesSource");

                        using (GTProperty_GetOnly key = cls.Generate_Property<GTProperty_GetOnly>())
                        {
                            key.SetRequired("byte[]", "Key", EKeyword.PUBLIC, EKeyword.OVERRIDE);

                            using (GTValue_ArrayInitializer ar = key.Generate_DefaultValue<GTValue_ArrayInitializer>())
                            {
                                ar.SetRequired("byte");

                                foreach(byte b in _aes.Key)
                                {
                                    ar.Generate_Value<GTValue>().SetRequired(b.ToString());
                                }
                            }
                        }

                        using (GTProperty_GetOnly iv = cls.Generate_Property<GTProperty_GetOnly>())
                        {
                            iv.SetRequired("byte[]", "Iv", EKeyword.PUBLIC, EKeyword.OVERRIDE);

                            using (GTValue_ArrayInitializer ar = iv.Generate_DefaultValue<GTValue_ArrayInitializer>())
                            {
                                ar.SetRequired("byte");

                                foreach (byte b in _aes.IV)
                                {
                                    ar.Generate_Value<GTValue>().SetRequired(b.ToString());
                                }
                            }
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
