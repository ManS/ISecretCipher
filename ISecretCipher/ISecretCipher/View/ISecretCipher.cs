using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SecretCipher.Model.Encryption;
using SecretCipher.Model.Keys;
using SecretCipher.Model.Decryption;
using SecretCipher.Utilities;
using SecretCipher.Model;
using ISecretCipher.Controller;
using ISecretCipher.Utilities;

namespace ISecretCipher
{
    public partial class ISecretCipher : Form, IUserObserver
    {
        
        #region Properties
        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public UserController Controller { get; set; }
        private Label[,] diagram = new Label[5, 5];
        private ISecretCipherAlgorithms currectAlgorithm = ISecretCipherAlgorithms.Caesar;
        private Mode currentMode = Mode.Encryption;
        #endregion

        #region Constructor
        public ISecretCipher()
        {
            InitializeComponent();
            
            this.IntializeConfigurations();
        }
        #endregion

        #region IUserObserver Functions
        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="p_errorMsg">The p_error MSG.</param>
        public void OnMessage(string p_errorMsg)
        {
            toolstrip.Text = p_errorMsg;
        }
        /// <summary>
        /// Called when [message encrypted].
        /// </summary>
        /// <param name="p_encryptedMsg">The p_encrypted MSG.</param>
        public void OnMessageEncrypted(string p_encryptedMsg)
        {
            cipherText_richbox.Text = p_encryptedMsg;
            toolstrip.Text = "Message Encrypted!";
        }
        /// <summary>
        /// Called when [message decrypted].
        /// </summary>
        /// <param name="p_decryptedMsg">The p_decrypted MSG.</param>
        public void OnMessageDecrypted(string p_decryptedMsg)
        {
            this.cipherText_richbox.Text = p_decryptedMsg;
            toolstrip.Text = "Message Decrypted!";
        }
        #endregion

        #region Controls Event Handlers
        private void algorithm_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ConfigureEnvironment(this.GetSelectedAlgorithm());
        }
        private void go_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.currentMode == Mode.Encryption)
                {
                    this.Encryption();
                }
                else
                    this.Decryption();
            }
            catch (System.Exception ex)
            {
                toolstrip.Text = ex.Message;
            }
            
        }
        private void random_btn_Click(object sender, EventArgs e)
        {
            try
            {
                MonoalphabeticKey tempKey = new MonoalphabeticKey();
                for (int i = 0; i < 26; i++)
                {
                    char currentChar = (char)((int)'a' + i);
                    ((TextBox)monoalphabetic_panel.Controls[currentChar.ToString() + "_txtbox"]).Text = tempKey.KeyValues[i].ToString();
                }
            }
            catch (System.Exception ex)
            {
                toolstrip.Text = ex.Message;
            }
            
        }
        private void clear_btn_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < 26; i++)
                {
                    char currentChar = (char)((int)'a' + i);
                    ((TextBox)monoalphabetic_panel.Controls[currentChar.ToString() + "_txtbox"]).Text = "";
                }
            }
            catch (System.Exception ex)
            {
                toolstrip.Text = ex.Message;
            }
            
        }
        private void playfair_key_txtbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                PlayFairKey tempKey = new PlayFairKey(playfair_key_txtbox.Text.ToUpper());
                for (int i = 0; i < 5; i++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if ((char)tempKey.KeyMatrix[i, k] != 'I')
                        {
                            diagram[i, k].Text = ((char)tempKey.KeyMatrix[i, k]).ToString();
                        }
                        else
                            diagram[i, k].Text = "I\\J";
                    }
                }
                toolstrip.Text = "";
                
            }
            catch
            {
                toolstrip.Text = "Chars must be between A-Z";
            }
            
        }
        private void columnar_keysize_ValueChanged(object sender, EventArgs e)
        {
            int i = 1;
            for (; i <= columnar_keysize.Value; i++)
            {
                this.columnar_panel.Controls["seq_" + i.ToString()].Show();
            }
            for (; i <= 7; i++)
            {
                this.columnar_panel.Controls["seq_" + i.ToString()].Hide();
            }
        }
        private void encrypt_radiobtn_CheckedChanged(object sender, EventArgs e)
        {
            this.currentMode = Mode.Encryption;
            this.plainText_richbox.Text = this.cipherText_richbox.Text;
            //this.cipherText_richbox.Text = "";
        }
        private void decrypt_radiobtn_CheckedChanged(object sender, EventArgs e)
        {
            this.currentMode = Mode.Decryption;
            this.plainText_richbox.Text = this.cipherText_richbox.Text;
            this.cipherText_richbox.Text = "";
        }
        private void brws_btn_Click(object sender, EventArgs e)
        {
            if (file_radiobtn.Checked)
            {
                IKey mykey = null;
                switch (currectAlgorithm)
                {
                    case ISecretCipherAlgorithms.AES:
                        mykey = this.GetAESKey();
                        break;
                    default:
                        toolstrip.Text = this.currectAlgorithm.ToString()+ " doesn't support file encryption!";
                        return;
                }
                OpenFileDialog fd = new OpenFileDialog();
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    SaveFileDialog sd = new SaveFileDialog();
                    if (sd.ShowDialog() == DialogResult.OK)
                    {
                        if (this.currentMode == Mode.Encryption)
                        {
                            this.Controller.EncryptFile(fd.FileName, sd.FileName, currectAlgorithm, mykey);
                        }
                        else
                            this.Controller.DecryptFile(fd.FileName, sd.FileName, currectAlgorithm, mykey);
                    }
                }    
            }
        }
        private void exchange_key_Click(object sender, EventArgs e)
        {
            try
            {
                EllpiticPoint G = new EllpiticPoint(int.Parse(gx_ec_txtbox.Text), int.Parse(gy_ec_txtbox.Text), int.Parse(q_ec_txtbox.Text));
                EllpiticCurve curve = new EllpiticCurve(int.Parse(a_ec_txtbox.Text), int.Parse(b_ec_txtbox.Text), int.Parse(q_ec_txtbox.Text));
                EllpiticUser user1 = new EllpiticUser(curve, int.Parse(na_ec_txtbox.Text), G, int.Parse(k_ec_txtbox.Text));
                EllpiticUser user2 = new EllpiticUser(curve, int.Parse(nb_ec_txtbox.Text), G, int.Parse(k_ec_txtbox.Text));
                a_ec_publickey_txtbox.Text = user1.PublicKey.ToString();
                b_ec_publickey_txtbox.Text = user2.PublicKey.ToString();
                a_ec_privatekey_txtbox.Text = user1.CalculateSharedKey(user2.PublicKey).ToString();
                b_ec_privatekey_txtbox.Text = user1.CalculateSharedKey(user2.PublicKey).ToString();
            }
            catch
            {
                toolstrip.Text = "Invalid Input!..";
            }
        }
        private void ExchangeKey_btn_Click(object sender, EventArgs e)
        {
            try
            {
                DiffHellmanKeyExchange k = new DiffHellmanKeyExchange(int.Parse(deffieHellman_q_txtbox.Text), int.Parse(deffieHellman_a_txtbox.Text), int.Parse(deffieHellman_xa_txtbox.Text), int.Parse(deffieHellman_xb_txtbox.Text));
                double[] key = k.ExchangeKeys();
                if (key[0] == key[1])
                {
                    deffieHellman_ka_txtbox.Text = key[0].ToString();
                    deffieHellman_kb_txtbox.Text = key[1].ToString();
                }
            }
            catch
            {
                toolstrip.Text = "Invalid Format!";
            }
        }
        private void calMulInv_btn_Click(object sender, EventArgs e)
        {
            try
            {
                mulinv_txtbox.Text = Toolbox.GetInverseMod(int.Parse(num_base_txtbox.Text), int.Parse(num_mulinv_txtbox.Text)).ToString();
                toolstrip.Text = "Done!";
            }
            catch (System.Exception ex)
            {
                toolstrip.Text = ex.Message;
            }
        }
        #endregion

        #region Read Keys
        private IKey    GetRSAKey()
        {
            return new RSAKey(int.Parse(rsa_p_txtbox.Text), int.Parse(rsa_q_txtbox.Text), int.Parse(rsa_e_txtbox.Text));
        }
        private IKey    GetTripleDESKey()
        {
            TripleDESMode mode;
            DESKey[] keys;
            KeyType type = this.GetTripleDesKeyType();
            if (tripleDeskey_mode_cb.SelectedIndex == 0)
            {
                mode = TripleDESMode.ThreekeysMode;
                keys = new DESKey[3];
                keys[0] = new DESKey(tripleDes_key1_txtbox.Text, type);
                keys[1] = new DESKey(tripleDes_key2_txtbox.Text, type);
                keys[2] = new DESKey(tripleDes_key3_txtbox.Text, type);
            }
            else
            {
                mode = TripleDESMode.TwoKeysMode;
                keys = new DESKey[2];
                keys[0] = new DESKey(tripleDes_key1_txtbox.Text, type);
                keys[1] = new DESKey(tripleDes_key2_txtbox.Text, type);
            }
            return new TripleDESKey(keys, mode);
        }
        private KeyType GetTripleDesKeyType()
        {
            if (tripleDesKeyformat_cb.SelectedIndex == 0)
            {
                return KeyType.ASCII;
            }
            return KeyType.Hex;
        }
        private IKey    GetRC4Key()
        {
            List<char> sequ = new List<char>();
            for (int i = 0; i < rc4_key_txtbox.Text.Length; i++)
            {
                sequ.Add(rc4_key_txtbox.Text[i]);
            }
            return new Rc4Key(sequ);
        }
        private IKey    GetAESKey()
        {
            return new AESKey(KeySize.x128Bits, aes_key_txtbox.Text, this.GetAESKeyType());
        }
        private KeyType GetAESKeyType()
        {
            if (aes_keyformat_cb.SelectedIndex == 0)
            {
                return KeyType.ASCII;
            }
            return KeyType.Hex;
        }
        private IKey    GetDesKey()
        {
            DESKey mykey = new DESKey(des_key_txtbox.Text, this.GetDesInputFormat());
            if (mykey.ValidateKey() == ValidationResponse.Sufficient)
            {
                for (int i = 1; i <= 16; i++)
                {
                    ((Label)des_panel.Controls["des_key" + i.ToString()]).Text = "Key " + i.ToString() + " =" + Toolbox.BinaryBytesArrayToString(mykey.Keys[i - 1]);
                }
            }
            else
            {

            }
            return mykey;
        }
        private KeyType GetDesInputFormat()
        {
            if (des_keytype_cb.SelectedIndex == 0)
            {
                return KeyType.ASCII;
            }
            else
                return KeyType.Hex;
        }
        private IKey    GetColumnarKey()
        {
            List<int> sequence = new List<int>();
            for (int i = 1; i <= columnar_keysize.Value; i++)
            {
                sequence.Add((int)((NumericUpDown)columnar_panel.Controls["seq_" + i.ToString()]).Value);
            }
            return new ColumnarKey(sequence);
        }
        private IKey    GetRailFence()
        {
            return new RailFenceKey((int)railfence_depthlevel.Value);
        }
        private IKey    GetHillCipherKey()
        {
            return new HillCipherKey((int)hill_blocksize.Value, hill_key_txtbox.Text.ToUpper());
        }
        private IKey    GetPlayFairKey()
        {
            return new PlayFairKey(playfair_key_txtbox.Text.ToUpper());
        }
        private InputType GetInputType()
        {
            if (ascii_radio.Checked)
                return InputType.ASCII;
            else if (hex_radio.Checked)
                return InputType.HEX;
            else
                return InputType.File;
        }
        private IKey    GetMonoalphabeticKey()
        {
            char[] keyValues = new char[26];
            try
            {
                for (int i = 0; i < 26; i++)
                {
                    keyValues[i] = ((TextBox)monoalphabetic_panel.Controls[((char)(i + 65)).ToString().ToLower() + "_txtbox"]).Text.ToUpper()[0];
                }
                //mykey = 
                return new MonoalphabeticKey(keyValues);
            }
            catch
            {
                toolstrip.Text = "Invalid Key!";
            }
            return null;

        }
        private IKey    GetCaeserKey()
        {
            try
            {
                return new CaesarKey(int.Parse(caeser_key.Text));
            }
            catch
            {
                toolstrip.Text = "Key Must be between 0-27";
            }
            return null;
        }
        private IKey    GetVigenere()
        {
            if (repeatingkey_radio.Checked)
            {
                return new PolyalphabeticKey(PolyKeyType.Repeat, vigenere_key_txtbox.Text);
            }
            return new PolyalphabeticKey(PolyKeyType.Auto, vigenere_key_txtbox.Text);
        }
        #endregion

        #region Decryption Encryption Functions
        private void Decryption()
        {
            IKey mykey = null;
            switch (this.currectAlgorithm)
            {
                case ISecretCipherAlgorithms.Caesar:
                    mykey = this.GetCaeserKey();
                    break;
                case ISecretCipherAlgorithms.Monoalphabetic:
                    mykey = this.GetMonoalphabeticKey();
                    break;
                case ISecretCipherAlgorithms.Vigenere:
                    mykey = this.GetVigenere();
                    break;
                case ISecretCipherAlgorithms.PlayFair:
                    mykey = this.GetPlayFairKey();
                    break;
                case ISecretCipherAlgorithms.HillCipher:
                    mykey = this.GetHillCipherKey();
                    break;
                case ISecretCipherAlgorithms.RailFence:
                    mykey = this.GetRailFence();
                    break;
                case ISecretCipherAlgorithms.Columnar:
                    mykey = this.GetColumnarKey();
                    break;
                case ISecretCipherAlgorithms.DES:
                    mykey = this.GetDesKey();
                    break;
                case ISecretCipherAlgorithms.AES:
                    mykey = this.GetAESKey();
                    break;
                case ISecretCipherAlgorithms.RC4:
                    mykey = this.GetRC4Key();
                    break;
                case ISecretCipherAlgorithms.TripleDES:
                    mykey = this.GetTripleDESKey();
                    break;
                case ISecretCipherAlgorithms.RSA:
                    this.ApplyRSADecryption();
                    return;
                case ISecretCipherAlgorithms.DeffieHellman:
                    return;
                case ISecretCipherAlgorithms.EllipticCurve:
                    this.DecryptEllipticCurve();
                    return;
                case ISecretCipherAlgorithms.MulInverse:
                    return;
                default:
                    return;
            }

            switch (this.GetInputType())
            {
                case InputType.ASCII:
                    this.Controller.DecryptASCII(plainText_richbox.Text, this.currectAlgorithm, mykey);
                    break;
                case InputType.HEX:
                    this.Controller.DecryptHex(plainText_richbox.Text, this.currectAlgorithm, mykey);
                    break;
                case InputType.File:
                    break;
                default:
                    break;
            }
        }
        private void ApplyRSADecryption()
        {
            try
            {
                RSAKey mykey = new RSAKey(int.Parse(rsa_p_txtbox.Text), int.Parse(rsa_q_txtbox.Text), int.Parse(rsa_e_txtbox.Text));
                decimal num = decimal.Parse(plainText_richbox.Text);
                RSADecryptor myEncryptor = new RSADecryptor(mykey);
                cipherText_richbox.Text = myEncryptor.DecryptNumber(num).ToString();
                toolstrip.Text = "Done!";
            }
            catch
            {
                toolstrip.Text = "Invalid Format!";
            }
        }
        private void DecryptEllipticCurve()
        {
            try
            {
                EllpiticPoint G = new EllpiticPoint(int.Parse(gx_ec_txtbox.Text), int.Parse(gy_ec_txtbox.Text), int.Parse(q_ec_txtbox.Text));
                EllpiticCurve curve = new EllpiticCurve(int.Parse(a_ec_txtbox.Text), int.Parse(b_ec_txtbox.Text), int.Parse(q_ec_txtbox.Text));
                EllpiticUser user1 = new EllpiticUser(curve, int.Parse(na_ec_txtbox.Text), G, int.Parse(k_ec_txtbox.Text));
                EllpiticUser user2 = new EllpiticUser(curve, int.Parse(nb_ec_txtbox.Text), G, int.Parse(k_ec_txtbox.Text));
                a_ec_publickey_txtbox.Text = user1.PublicKey.ToString();
                b_ec_publickey_txtbox.Text = user2.PublicKey.ToString();
                a_ec_privatekey_txtbox.Text = user1.CalculateSharedKey(user2.PublicKey).ToString();
                b_ec_privatekey_txtbox.Text = user1.CalculateSharedKey(user2.PublicKey).ToString();
                //EllpiticPoint PlainData = Toolbox.StringToPoint(plainText_richbox.Text.Trim(), G.P);

                List<EllpiticPoint> CT = Toolbox.GetCipherElliptic(plainText_richbox.Text.Trim(), G.P);
                EllpiticPoint decryptedPoint = user2.Decrypt(CT[0], CT[1]);
                cipherText_richbox.Text = decryptedPoint.ToString();
                toolstrip.Text = "Done!";
            }
            catch
            {
                toolstrip.Text = "Invalid Input!";
            }
        }
        private void Encryption()
        {
            IKey mykey = null;
            switch (this.GetSelectedAlgorithm())
            {
                case ISecretCipherAlgorithms.Caesar:
                    {
                        mykey = this.GetCaeserKey();
                        if (mykey == null)
                            return;
                    }
                    break;
                case ISecretCipherAlgorithms.Monoalphabetic:
                    {
                        mykey = this.GetMonoalphabeticKey();
                        if (mykey == null)
                        {
                            return;
                        }
                    }
                    break;
                case ISecretCipherAlgorithms.Vigenere:
                    mykey = this.GetVigenere();
                    break;
                case ISecretCipherAlgorithms.PlayFair:
                    mykey = this.GetPlayFairKey();
                    break;
                case ISecretCipherAlgorithms.HillCipher:
                    mykey = this.GetHillCipherKey();
                    break;
                case ISecretCipherAlgorithms.RailFence:
                    mykey = this.GetRailFence();
                    break;
                case ISecretCipherAlgorithms.Columnar:
                    mykey = this.GetColumnarKey();
                    break;
                case ISecretCipherAlgorithms.DES:
                    mykey = this.GetDesKey();
                    break;
                case ISecretCipherAlgorithms.AES:
                    mykey = this.GetAESKey();
                    break;
                case ISecretCipherAlgorithms.RC4:
                    mykey = this.GetRC4Key();
                    break;
                case ISecretCipherAlgorithms.TripleDES:
                    mykey = this.GetTripleDESKey();
                    break;
                case ISecretCipherAlgorithms.RSA:
                    this.ApplyRSAEncryption();
                    return;
                case ISecretCipherAlgorithms.DeffieHellman:
                    return;
                case ISecretCipherAlgorithms.EllipticCurve:
                    this.EllipticCurveEncryption();
                    return;
                case ISecretCipherAlgorithms.MulInverse:
                    return;
                default:
                    return;
            }
            switch (this.GetInputType())
            {
                case InputType.ASCII:
                    this.Controller.EncryptASCII(this.plainText_richbox.Text, this.currectAlgorithm, mykey);
                    break;
                case InputType.HEX:
                    this.Controller.EncryptHex(this.plainText_richbox.Text, this.currectAlgorithm, mykey);
                    break;
                case InputType.File:
                    {

                    }
                    break;
                default:
                    break;
            }
        }
        private void ApplyRSAEncryption()
        {
            try
            {
                RSAKey mykey = new RSAKey(int.Parse(rsa_p_txtbox.Text), int.Parse(rsa_q_txtbox.Text), int.Parse(rsa_e_txtbox.Text));
                decimal num = decimal.Parse(plainText_richbox.Text);
                RSAEncryptor myEncryptor = new RSAEncryptor(mykey);
                cipherText_richbox.Text = myEncryptor.EncryptNumber(num).ToString();
                toolstrip.Text = "Done!";
            }
            catch
            {
                toolstrip.Text = "Invalid Format!";
            }
        }
        private void EllipticCurveEncryption()
        {
            try
            {
                EllpiticPoint G = new EllpiticPoint(int.Parse(gx_ec_txtbox.Text), int.Parse(gy_ec_txtbox.Text), int.Parse(q_ec_txtbox.Text));
                EllpiticCurve curve = new EllpiticCurve(int.Parse(a_ec_txtbox.Text), int.Parse(b_ec_txtbox.Text), int.Parse(q_ec_txtbox.Text));
                EllpiticUser user1 = new EllpiticUser(curve, int.Parse(na_ec_txtbox.Text), G, int.Parse(k_ec_txtbox.Text));
                EllpiticUser user2 = new EllpiticUser(curve, int.Parse(nb_ec_txtbox.Text), G, int.Parse(k_ec_txtbox.Text));
                a_ec_publickey_txtbox.Text = user1.PublicKey.ToString();
                b_ec_publickey_txtbox.Text = user2.PublicKey.ToString();
                a_ec_privatekey_txtbox.Text = user1.CalculateSharedKey(user2.PublicKey).ToString();
                b_ec_privatekey_txtbox.Text = user1.CalculateSharedKey(user2.PublicKey).ToString();
                EllpiticPoint PlainData = Toolbox.StringToEllipticPoint(plainText_richbox.Text.Trim(), G.P);

                List<EllpiticPoint> CT = user1.Encrypt(PlainData, user2.PublicKey);

                cipherText_richbox.Text = CT[0].ToString() + ";" + CT[1].ToString();
                toolstrip.Text = "Done!";
            }
            catch
            {
                toolstrip.Text = "Invalid Input!..";
            }
        }
        #endregion

        #region Helper Functions
        /// <summary>
        /// Initializes the configurations.
        /// </summary>
        private void IntializeConfigurations()
        {
            this.Controller = new UserController();
            this.Controller.Register(this);
            mulinv_panel.Parent = configurations_gb;
            mulinv_panel.Location = new Point(12, 15);
            algorithm_combobox.SelectedIndex = 0;
            hillcipher_panel.Parent = configurations_gb;
            hillcipher_panel.Location = new Point(12, 15);
            playfair_panel.Parent = configurations_gb;
            playfair_panel.Location = new Point(12, 15);
            caeser_panel.Parent = configurations_gb;
            caeser_panel.Location = new Point(12, 15);
            vigenere_panel.Parent = configurations_gb;
            vigenere_panel.Location = new Point(12, 15);
            aes_panel.Parent = configurations_gb;
            aes_panel.Location = new Point(12, 15);
            monoalphabetic_panel.Parent = configurations_gb;
            monoalphabetic_panel.Location = new Point(12, 15);
            ellipticcurve_panel.Parent = configurations_gb;
            ellipticcurve_panel.Location = new Point(12, 15);
            columnar_panel.Parent = configurations_gb;
            columnar_panel.Location = new Point(12, 15);
            des_panel.Parent = configurations_gb;
            des_panel.Location = new Point(12, 15);
            rsa_panel.Parent = configurations_gb;
            rsa_panel.Location = new Point(12, 15);
            railfence_panel.Parent = configurations_gb;
            railfence_panel.Location = new Point(12, 15);
            tripleDes_panel.Parent = configurations_gb;
            tripleDes_panel.Location = new Point(12, 15);
            deffiehellman_panel.Parent = configurations_gb;
            deffiehellman_panel.Location = new Point(12, 15);
            rc4_panel.Parent = configurations_gb;
            rc4_panel.Location = new Point(12, 15);
            des_keytype_cb.SelectedIndex = 0;
            aes_keyformat_cb.SelectedIndex = 0;
           
            this.InitializeDiagram();
        }
        /// <summary>
        /// Initializes the diagram.
        /// </summary>
        private void InitializeDiagram()
        {
            int k=1;
            for (int i = 0; i < 5; i++ )
            {
                for (int j = 0; j < 5; j++ )
                {
                    diagram[i,j] = (Label)diagram_panel.Controls["playfair" + k.ToString()+"_lbl"];
                    k++;
                }
            }
        }
        /// <summary>
        /// Gets the selected algorithm.
        /// </summary>
        /// <returns></returns>
        private ISecretCipherAlgorithms GetSelectedAlgorithm()
        {
            switch (algorithm_combobox.SelectedIndex)
            {
                case 0:
                    return ISecretCipherAlgorithms.Caesar;
                case 1:
                    return ISecretCipherAlgorithms.Monoalphabetic;
                case 2:
                    return ISecretCipherAlgorithms.Vigenere;
                case 3:
                    return ISecretCipherAlgorithms.PlayFair;
                case 4:
                    return ISecretCipherAlgorithms.HillCipher;
                case 5:
                    return ISecretCipherAlgorithms.RailFence;
                case 6:
                    return ISecretCipherAlgorithms.Columnar;
                case 7:
                    return ISecretCipherAlgorithms.DES;
                case 8:
                    return ISecretCipherAlgorithms.AES;
                case 9:
                    return ISecretCipherAlgorithms.RC4;
                case 10:
                    return ISecretCipherAlgorithms.TripleDES;
                case 11:
                    return ISecretCipherAlgorithms.RSA;
                case 12:
                    return ISecretCipherAlgorithms.DeffieHellman;
                case 13:
                    return ISecretCipherAlgorithms.EllipticCurve;
                default:
                    return ISecretCipherAlgorithms.MulInverse;
            }
        }
        /// <summary>
        /// Configures the environment.
        /// </summary>
        /// <param name="iSecretCipherAlgorithms">The i secret cipher algorithms.</param>
        private void ConfigureEnvironment(ISecretCipherAlgorithms iSecretCipherAlgorithms)
        {
            this.HideAll();
            this.currectAlgorithm = this.GetSelectedAlgorithm();
            switch (iSecretCipherAlgorithms)
            {
                case ISecretCipherAlgorithms.Caesar:
                    this.caeser_panel.Show();
                    this.caeser_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.Monoalphabetic:
                    this.monoalphabetic_panel.Show();
                    this.monoalphabetic_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.Vigenere:
                    this.vigenere_panel.Show();
                    this.vigenere_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.PlayFair:
                    this.playfair_panel.Show();
                    this.playfair_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.HillCipher:
                    this.hillcipher_panel.Show();
                    this.hillcipher_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.RailFence:
                    railfence_panel.Show();
                    railfence_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.Columnar:
                    this.columnar_panel.Show();
                    this.columnar_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.DES:
                    des_panel.Show();
                    this.des_panel.BringToFront();
                    this.des_keytype_cb.SelectedIndex = 0;
                    break;
                case ISecretCipherAlgorithms.AES:
                    aes_panel.Show();
                    this.aes_panel.BringToFront();
                    this.aes_keyformat_cb.SelectedIndex = 0;
                    break;
                case ISecretCipherAlgorithms.RC4:
                    rc4_panel.Show();
                    rc4_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.TripleDES:
                    tripleDes_panel.Show();
                    tripleDes_panel.BringToFront();
                    tripleDeskey_mode_cb.SelectedIndex = 0;
                    tripleDesKeyformat_cb.SelectedIndex = 0;
                    break;
                case ISecretCipherAlgorithms.RSA:
                    this.rsa_panel.Show();
                    this.rsa_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.DeffieHellman:
                    deffiehellman_panel.Show();
                    deffiehellman_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.EllipticCurve:
                    this.ellipticcurve_panel.Show();
                    this.ellipticcurve_panel.BringToFront();
                    break;
                case ISecretCipherAlgorithms.MulInverse:
                    this.mulinv_panel.Show();
                    this.mulinv_panel.BringToFront();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Hides all.
        /// </summary>
        private void HideAll()
        {

            hillcipher_panel.Hide();
            playfair_panel.Hide();
            caeser_panel.Hide();
            vigenere_panel.Hide();
            aes_panel.Hide();
            monoalphabetic_panel.Hide();
            ellipticcurve_panel.Hide();
            columnar_panel.Hide();
            des_panel.Hide();
            rsa_panel.Hide();
            railfence_panel.Hide();
            tripleDes_panel.Hide();
            deffiehellman_panel.Hide();
            rc4_panel.Hide();
            mulinv_panel.Hide();
        }

        #endregion        

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Team Members:\n1- Yassmin Taha\n2- Rehab Reda\n3- Kariem Mohammed\n4- Ahmad Mansour");
        }
    }
}