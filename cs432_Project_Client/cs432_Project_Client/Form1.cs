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
        byte[] message;
        byte[] encryptedRSA;
        string messageStr;

        bool terminating = false;
        bool connected = false;
        Socket clientSocket;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
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
            string pass = password.Text;
            sha256 = hashWithSHA256(pass);
            concatenateHashWithUsername();
            encryptMessage();
            sendEncryptedMessage();
        }

        private void encryptMessage()
        {
            //messageStr = generateHexStringFromByteArray(message);
            encryptedRSA = encryptWithRSA(messageStr, 3072, RSAPublicKey3072_encryption);
            Console.WriteLine("RSA 3072 Encryption:");
            Console.WriteLine(generateHexStringFromByteArray(encryptedRSA));
            Console.WriteLine(encryptedRSA.Length);

        }
        public static string generateHexStringFromByteArray(byte[] input)
        {
            string hexString = BitConverter.ToString(input);
            return hexString.Replace("-", "");
        }
        private void concatenateHashWithUsername()
        {
            int halfLength = sha256.Length / 2;
            byte[] halfHash = new byte[halfLength];
            Console.WriteLine("sha length: " + sha256.Length);
           
            Array.Copy(sha256, halfLength, halfHash, 0, halfLength);
         
            messageStr = username.Text + "/" + generateHexStringFromByteArray(halfHash);

            Console.WriteLine("messagestr: " + messageStr);
        }

        private void Receive()
        {
            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[384];
                    clientSocket.Receive(buffer);
                    
                    string msg = "success";
                    if (verifyWithRSA(msg, 3072, RSAPublicKey3072_verification, buffer))
                    {
                        logs.AppendText("Enrollment Successfull\n");
                        loginButton.Enabled = true;
                    }
                    else
                        logs.AppendText("Enrollment Failed\n");

                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected\n");
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
            //String message = messageText.Text;
            string username = usernameLogin.Text;
            string password = passwordLogin.Text;

            if (username != "" && password != "" && message.Length < 63)
            {
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes(username);
                clientSocket.Send(buffer);
            }
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
        private void sendEncryptedMessage()
        {
            //clientSocket.Send(message);
            //String message = messageText.Text;

            //Byte[] buffer = new Byte[512];
            //buffer = Encoding.Default.GetBytes(messageStr);
            //clientSocket.Send(buffer);
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

    }
}
