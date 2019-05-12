using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;

namespace cs432_Project_Client
{
    public partial class Form1 : Form
    {
        string RSAPublicKey3072_encryption;
        string RSAPublicKey3072_verification;
        byte[] sha256;
        //byte[] message;
        byte[] encryptedRSA;
        byte[] halfHash;
        byte[] sessionKeyEnc;
        byte[] sessionKeyAuth;
        string messageStr;
        string userName;

        string challenge;
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
            ipAdress.Text = "localhost";
            portNum.Text = "123";
            RSAPublicKey3072_encryption = readRSAPublicKey_encryption();
            RSAPublicKey3072_verification = readRSAPublicKey_verification();

        }
        private void connectButton_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = ipAdress.Text;
            int port;
            if (Int32.TryParse(portNum.Text, out port))
            {
                try
                {
                    clientSocket.Connect(IP, port);
                    enrollButton.Enabled = true;
                    loginButton.Enabled = true;
                    connectButton.Enabled = false;
                    connected = true;
                    logs.AppendText("Connected to server\n");

                    Thread receiveThread = new Thread(new ThreadStart(Receive));
                    receiveThread.Start();

                }
                catch
                {
                    logs.AppendText("Could not connect to server\n");
                }
            }
            else
            {
                logs.AppendText("Check the port\n");
            }
        }
        private void enrollButton_Click(object sender, EventArgs e)
        {
            if (username.Text != "" && password.Text != "")
            {
                string pass = password.Text;
                sha256 = hashWithSHA256(pass);
                concatenateHashWithUsername();
                encryptMessage();
                sendEncryptedMessage();
            }
            else
            {
                logs.AppendText("Enter valid username/password\n");

            }
        }

        private void encryptMessage()
        {
            try
            {
                //messageStr = generateHexStringFromByteArray(message);
                encryptedRSA = encryptWithRSA(messageStr, 3072, RSAPublicKey3072_encryption);
                Console.WriteLine("RSA 3072 Encryption:");
                Console.WriteLine(generateHexStringFromByteArray(encryptedRSA));
                Console.WriteLine(encryptedRSA.Length);
            }
            catch
            {
                logs.AppendText("Encryption Failed\n");
            }


        }
        public static string generateHexStringFromByteArray(byte[] input)
        {
            string hexString = BitConverter.ToString(input);
            return hexString.Replace("-", "");
        }
        private void concatenateHashWithUsername()
        {
            int halfLength = sha256.Length / 2;
            halfHash = new byte[halfLength];
            Console.WriteLine("sha length: " + sha256.Length);

            Array.Copy(sha256, halfLength, halfHash, 0, halfLength);

            messageStr = username.Text + "/" + Encoding.Default.GetString(halfHash);

            Console.WriteLine("messagestr: " + messageStr);
        }

        private void Receive()
        {
            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[386];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    //string incomingMessage = Convert.ToBase64String(buffer);
                    string messageCode = incomingMessage.Substring(0, 2);
                    incomingMessage = incomingMessage.Substring(2);

                    if (messageCode == "/E")
                    {
                        byte[] arr = Encoding.Default.GetBytes(incomingMessage);
                        string msg = "success";
                        if (verifyWithRSA(msg, 3072, RSAPublicKey3072_verification, arr))
                        {
                            logs.AppendText("Enrollment Successfull\n");
                            if (loginButton.Enabled != true)
                                loginButton.Enabled = true;
                        }
                        else
                            logs.AppendText("Enrollment Failed\n");
                    }
                    else if (messageCode == "/A")
                    {
                        try
                        {
                            //incomingMessage = Encoding.Default.GetString(buffer);
                            incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0\0"));
                            challenge = incomingMessage;
                            string password = passwordLogin.Text;
                            byte[] passwordHash = hashWithSHA256(password);
                            sha256 = passwordHash;
                            int halfLength = passwordHash.Length / 2;
                            halfHash = new byte[halfLength];

                            Array.Copy(passwordHash, halfLength, halfHash, 0, halfLength);
                            byte[] hmac = applyHMACwithSHA256(incomingMessage, halfHash);

                            string str = Encoding.Default.GetString(hmac);
                            str = "/H" + str;
                            byte[] bffr = Encoding.Default.GetBytes(str);
                            clientSocket.Send(bffr);
                        }
                        catch
                        {
                            logs.AppendText("HMAC Failed\n");
                        }
                    }
                    else if (messageCode == "/M")
                    {
                        try
                        {

                            string unsigned = incomingMessage.Substring(0, incomingMessage.IndexOf("|||"));
                            string signed = incomingMessage.Substring(incomingMessage.IndexOf("|||") + 3);

                            byte[] arr = Encoding.Default.GetBytes(signed);
                            //string msg = "success";
                            try
                            {
                                verifyWithRSA(unsigned, 3072, RSAPublicKey3072_verification, arr);
                                string result = unsigned.Substring(0, incomingMessage.IndexOf("///"));
                                if (result == "success")
                                {
                                    logs.AppendText("Login Successfull\n");
                                    loginButton.Enabled = false;
                                    unsigned = unsigned.Substring(unsigned.IndexOf("///") + 3);
                                    //incomingMessage = incomingMessage.Substring(2);
                                    string sessionKey1 = unsigned.Substring(0, unsigned.IndexOf("///"));
                                    string sessionKey2 = unsigned.Substring(unsigned.IndexOf("///") + 3);
                                    byte[] byteChallenge = Encoding.Default.GetBytes(challenge);

                                    sessionKeyEnc = decryptWithAES128(sessionKey1, halfHash, byteChallenge);
                                    sessionKeyAuth = decryptWithAES128(sessionKey2, halfHash, byteChallenge);

                                    //sessionKey1 = Encoding.Default.GetString(sessionKeyEnc);
                                    //sessionKey2 = Encoding.Default.GetString(sessionKeyAuth);


                                    messageBox.Enabled = true;
                                    messageButton.Enabled = true;

                                }
                                else
                                {
                                    logs.AppendText("Login Failed\n");
                                }

                            }
                            catch
                            {
                                logs.AppendText("Login Failed\n");
                            }
                            // if (verifyWithRSA(unsigned, 3072, RSAPublicKey3072_verification, arr))
                            // {
                            //     logs.AppendText("Login Successfull\n");
                            //     loginButton.Enabled = true;
                            //     incomingMessage = incomingMessage.Substring(2);
                            //     string sessionKey1 = incomingMessage.Substring(0, incomingMessage.IndexOf("///"));
                            //     string encryptedsessionKeyAuth = incomingMessage.Substring(incomingMessage.IndexOf("///") + 1);
                            // }
                            // else
                            //     logs.AppendText("Login Failed\n");
                        }
                        catch
                        {
                            logs.AppendText("Verification Failed\n");
                        }
                    }
                    else if (messageCode == "/B")
                    {
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                        string encryptedMsg = incomingMessage.Substring(0, incomingMessage.IndexOf("///"));
                        string hmacMsg = incomingMessage.Substring(incomingMessage.IndexOf("///") + 3);

                        try
                        {
                            byte[] challengeByte = Encoding.Default.GetBytes(challenge);
                            byte[] decryptedMsg = decryptWithAES128(encryptedMsg, sessionKeyEnc, challengeByte);

                            byte[] hmac = applyHMACwithSHA256(encryptedMsg, sessionKeyAuth);
                            string hmacStr = Encoding.Default.GetString(hmac);
                            if (hmacStr == hmacMsg)
                            {
                                string decryptedMsgStr = Encoding.Default.GetString(decryptedMsg);
                                logs.AppendText(decryptedMsgStr + "\n");
                            }
                        }
                        catch
                        {
                            logs.AppendText("error occured.\n");
                        }

                    }

                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected\n");
                        connectButton.Enabled = true;
                        enrollButton.Enabled = false;
                        loginButton.Enabled = false;
                        messageBox.Enabled = false;
                        messageButton.Enabled = false;
                    }

                    clientSocket.Close();
                    connected = false;
                }
            }
        }
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameLogin.Text;
            string password = passwordLogin.Text;

            if (username != "" && password != "")
            {
                string pass = passwordLogin.Text;
                sha256 = hashWithSHA256(pass);


                userName = username;
                Byte[] buffer = new Byte[386];
                string msg = "/A" + username;
                buffer = Encoding.Default.GetBytes(msg);
                clientSocket.Send(buffer);
            }
            else
            {
                logs.AppendText("Enter Valid Username/Password\n");
            }
        }
        static byte[] decryptWithAES128(string input, byte[] key, byte[] IV)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);

            // create AES object from System.Security.Cryptography
            RijndaelManaged aesObject = new RijndaelManaged();
            // since we want to use AES-128
            aesObject.KeySize = 128;
            // block size of AES is 128 bits
            aesObject.BlockSize = 128;
            // mode -> CipherMode.*
            aesObject.Mode = CipherMode.CFB;
            // feedback size should be equal to block size
            // aesObject.FeedbackSize = 128;
            // set the key
            aesObject.Key = key;
            // set the IV
            aesObject.IV = IV;
            // create an encryptor with the settings provided
            ICryptoTransform decryptor = aesObject.CreateDecryptor();
            byte[] result = null;

            try
            {
                result = decryptor.TransformFinalBlock(byteInput, 0, byteInput.Length);
            }
            catch (Exception e) // if encryption fails
            {
                Console.WriteLine(e.Message); // display the cause
            }

            return result;
        }

        private string readRSAPublicKey_encryption()
        {
            string RSAPublicKey3072;
            using (System.IO.StreamReader fileReader = new System.IO.StreamReader("../server_enc_dec_pub.txt"))
            {
                RSAPublicKey3072 = fileReader.ReadLine();
            }
            return RSAPublicKey3072;
        }
        private string readRSAPublicKey_verification()
        {
            string RSAPublicKey3072;
            using (System.IO.StreamReader fileReader = new System.IO.StreamReader("../server_signing_verification_pub.txt"))
            {
                RSAPublicKey3072 = fileReader.ReadLine();
            }
            return RSAPublicKey3072;
        }
        static byte[] hashWithSHA256(string input)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);
            // create a hasher object from System.Security.Cryptography
            SHA256CryptoServiceProvider sha256Hasher = new SHA256CryptoServiceProvider();
            // hash and save the resulting byte array
            byte[] result = sha256Hasher.ComputeHash(byteInput);

            return result;
        }

        // HMAC with SHA-256
        static byte[] applyHMACwithSHA256(string input, byte[] key)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);
            // create HMAC applier object from System.Security.Cryptography
            HMACSHA256 hmacSHA256 = new HMACSHA256(key);
            // get the result of HMAC operation
            byte[] result = hmacSHA256.ComputeHash(byteInput);

            return result;
        }

        private void sendEncryptedMessage()
        {
            //clientSocket.Send(message);
            //String message = messageText.Text;

            //Byte[] buffer = new Byte[512];
            //buffer = Encoding.Default.GetBytes(messageStr);
            //clientSocket.Send(buffer);
            string str = Encoding.Default.GetString(encryptedRSA);
            str = "/E" + str;
            encryptedRSA = Encoding.Default.GetBytes(str);
            Console.WriteLine(encryptedRSA.Length);
            clientSocket.Send(encryptedRSA);
        }
        static byte[] encryptWithRSA(string input, int algoLength, string xmlStringKey)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);
            // create RSA object from System.Security.Cryptography
            RSACryptoServiceProvider rsaObject = new RSACryptoServiceProvider(algoLength);
            // set RSA object with xml string
            rsaObject.FromXmlString(xmlStringKey);
            byte[] result = null;

            try
            {
                //true flag is set to perform direct RSA encryption using OAEP padding
                result = rsaObject.Encrypt(byteInput, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }
        static byte[] encryptWithAES128(string input, byte[] key, byte[] IV)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);

            // create AES object from System.Security.Cryptography
            RijndaelManaged aesObject = new RijndaelManaged();
            // since we want to use AES-128
            aesObject.KeySize = 128;
            // block size of AES is 128 bits
            aesObject.BlockSize = 128;
            // mode -> CipherMode.*
            aesObject.Mode = CipherMode.CFB;
            // feedback size should be equal to block size
            aesObject.FeedbackSize = 128;
            // set the key
            aesObject.Key = key;
            // set the IV
            aesObject.IV = IV;
            // create an encryptor with the settings provided
            ICryptoTransform encryptor = aesObject.CreateEncryptor();
            byte[] result = null;

            try
            {
                result = encryptor.TransformFinalBlock(byteInput, 0, byteInput.Length);
            }
            catch (Exception e) // if encryption fails
            {
                Console.WriteLine(e.Message); // display the cause
            }

            return result;
        }
        static bool verifyWithRSA(string input, int algoLength, string xmlString, byte[] signature)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);
            // create RSA object from System.Security.Cryptography
            RSACryptoServiceProvider rsaObject = new RSACryptoServiceProvider(algoLength);
            // set RSA object with xml string
            rsaObject.FromXmlString(xmlString);
            bool result = false;

            try
            {
                result = rsaObject.VerifyData(byteInput, "SHA256", signature);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void changePassButton_Click(object sender, EventArgs e)
        {


        }

        private void messageButton_Click(object sender, EventArgs e)
        {
            string message = messageBox.Text.ToString();
            byte[] iv = Encoding.Default.GetBytes(challenge);

            byte[] encMsg = encryptWithAES128(message, sessionKeyEnc, iv);
            string encryptedMessage = Encoding.Default.GetString(encMsg);

            byte[] hmac = applyHMACwithSHA256(encryptedMessage, sessionKeyAuth);
            string hmacMessage = Encoding.Default.GetString(hmac);

            string msgConcatanated = encryptedMessage + "///" + hmacMessage;
            msgConcatanated = "/B" + userName + "///" + msgConcatanated;
            byte[] buffer = Encoding.Default.GetBytes(msgConcatanated);

            clientSocket.Send(buffer);
        }
        private byte[] addCodeToMessage(byte[] arr, string s)
        {
            string str = Encoding.Default.GetString(arr);
            str = s + str;
            return Encoding.Default.GetBytes(str);
        }
    }
}
