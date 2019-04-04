﻿using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Diagnostics;
using System.Collections;

namespace cs432_Project_Server
{
    public partial class Form1 : Form
    {
      
        string RSAxmlKey3072;
        string RSASignVerifyKey;
        string password = "Bohemian";
        byte[] sha256;
        byte[] byteKey = new byte[16];
        byte[] byteIV = new byte[16];
        Hashtable userInfo = new Hashtable();
        string usrName;
        byte[] challenge;

        bool terminating = false;
        bool listening = false;
        //bool remoteConnected = false;

        
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Socket remoteSocket;
        List<Socket> socketList = new List<Socket>();
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
            sha256 = hashWithSHA256(password);
            getKeyAndIV();
            RSAxmlKey3072 = readRSAKeyPairs();
            RSASignVerifyKey = readRSASignVerify();
        }

        private void getKeyAndIV()
        {
            Array.Copy(sha256, 16, byteKey, 0, 16);
            Array.Copy(sha256, 0, byteIV, 0, 16);
        }

        //listenButton
        private void button1_Click(object sender, EventArgs e)
        {
            int serverPort;
            Thread acceptThread;

            if (Int32.TryParse(clientPort.Text, out serverPort))
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, serverPort));
                serverSocket.Listen(3);

                listening = true;
                listenButton.Enabled = false;
                acceptThread = new Thread(new ThreadStart(Accept));
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");
            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }
        private void connectButton_Click(object sender, EventArgs e)
        {
        //    remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    string IP = ipAdress.Text;
        //    int port;
        //    if (Int32.TryParse(portNum.Text, out port))
        //    {
        //        try
        //        {
        //            remoteSocket.Connect(IP, port);
        //            remoteConnected = true;
        //            connectButton.Enabled = false;
        //            logs.AppendText("Connected to remote math server\n");
        //        }
        //        catch
        //        {
        //            logs.AppendText("Could not connect to remote server\n");
        //        }
        //    }
        //    else
        //    {
        //        logs.AppendText("Check the port\n");
        //    }
        }
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }
        private void Accept()
        {
            while (listening)
            {
                try
                {
                    socketList.Add(serverSocket.Accept());
                    logs.AppendText("A client is connected \n");

                    Thread receiveThread;
                    receiveThread = new Thread(new ThreadStart(Receive));
                    receiveThread.Start();
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working \n");
                    }
                }
            }
        }
        private void Receive()
        {
            Socket s = socketList[socketList.Count - 1];
            bool connected = true;

            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[386];
                    s.Receive(buffer);

                    //string incomingMessage = generateHexStringFromByteArray(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    //string incomingMessage = Convert.ToBase64String(buffer);
                    string messageCode = incomingMessage.Substring(0, 2);
                    incomingMessage = incomingMessage.Substring(2);


                    Console.WriteLine(incomingMessage);
                    //incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    if (messageCode == "/E")
                    {
                        byte[] decryptedByteArray = decryptWithRSA(incomingMessage, 3072, RSAxmlKey3072);
                        string secretMessage = Encoding.Default.GetString(decryptedByteArray);

                        Console.WriteLine("Secret message:" + secretMessage);
                        string username;
                        string password;
                        username = secretMessage.Substring(0, secretMessage.IndexOf("/"));
                        password = secretMessage.Substring(secretMessage.IndexOf("/") + 1);

                        Console.WriteLine("username: " + username);
                        Console.WriteLine("password: " + password);

                        bool isUnique = true;
                        string responseMessage;
                        byte[] signedRSAmessage;
                        isUnique = checkUsernameUnique(username, password);
                        responseMessage = generateResponseMessage(isUnique);
                        signedRSAmessage = signResponseMessage(responseMessage);

                        string str = Encoding.Default.GetString(signedRSAmessage);
                        str = "/E" + str;
                        signedRSAmessage = Encoding.Default.GetBytes(str);
                        //buffer = null;
                        s.Send(signedRSAmessage);
                        //sendResponseMessage(signedRSAmessage);

                        //logs.AppendText(incomingMessage + "\n");
                    }
                    else if (messageCode == "/A")
                    {
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                        usrName = incomingMessage;

                        challenge = GenerateChallenge();

                        string str = Encoding.Default.GetString(challenge);
                        str = "/A" + str;
                        challenge = Encoding.Default.GetBytes(str);

                        s.Send(challenge);
                    }
                    else if (messageCode == "/H")
                    {
                        incomingMessage = incomingMessage.Substring (0, incomingMessage.IndexOf("\0"));
                        bool isVerified;
                        string pass = userInfo[usrName].ToString();
                        byte[] bytePass = Encoding.Default.GetBytes(pass);
                        string str = Encoding.Default.GetString(challenge);


                        if (Encoding.Default.GetString(applyHMACwithSHA256(str, bytePass)) == incomingMessage)
                            isVerified = true;
                        else isVerified = false;

                        if (isVerified)
                        {
                            logs.AppendText("Login Successful");
                        }
                    }

                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("A client has disconnected\n");
                    }

                    s.Close();
                    socketList.Remove(s);
                    connected = false;
                }
            }
        }

        private void sendResponseMessage(byte[] message)
        {
            Console.WriteLine("message length: " + message.Length);
            serverSocket.Send(message);
        }

        private byte[] signResponseMessage(string message)
        {
            byte[] signedRSA = signWithRSA(message, 3072, RSASignVerifyKey);
            return signedRSA;
        }

        private string generateResponseMessage(bool isUnique)
        {
            string message;
            if (isUnique)
            {
                message = "success";
            }
            else
                message = "error";
            return message;
        }

        private bool checkUsernameUnique(string username, string password)
        {
            if (!userInfo.Contains(username))
            {
                userInfo.Add(username, password);
                return true;
            }
            return false;
        }

        private string readRSAKeyPairs()
        {
            String encryptedString;
            string RSAKeyPairs;
            using (System.IO.StreamReader fileReader = new System.IO.StreamReader("../encrypted_server_enc_dec_pub_prv.txt"))
            {
                encryptedString = Encoding.Default.GetString(hexStringToByteArray(fileReader.ReadLine()));
                byte[] decryptedAES128 = decryptWithAES128(encryptedString, byteKey, byteIV);
                RSAKeyPairs = Encoding.Default.GetString(decryptedAES128);
                Console.WriteLine("AES128 Decryption Encr/Decr:");
                Console.WriteLine(RSAKeyPairs);
            }
            return RSAKeyPairs;
        }
        private string readRSASignVerify()
        {
            String encryptedString;
            string RSAKeyPairs;
            using (System.IO.StreamReader fileReader = new System.IO.StreamReader("../encrypted_server_signing_verification_pub_prv.txt"))
            {
                encryptedString = Encoding.Default.GetString(hexStringToByteArray(fileReader.ReadLine()));
                byte[] decryptedAES128 = decryptWithAES128(encryptedString, byteKey, byteIV);
                RSAKeyPairs = Encoding.Default.GetString(decryptedAES128);
                Console.WriteLine("AES128 Decryption Sign/Verify:");
                Console.WriteLine(RSAKeyPairs);
            }
            return RSAKeyPairs;
        }
        public static string generateHexStringFromByteArray(byte[] input)
        {
            string hexString = BitConverter.ToString(input);
            return hexString.Replace("-", "");
        }
        public static byte[] hexStringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
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
        // hash function: SHA-256
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
        static byte[] decryptWithRSA(string input, int algoLength, string xmlStringKey)
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
                result = rsaObject.Decrypt(byteInput, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }
        static byte[] signWithRSA(string input, int algoLength, string xmlString)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);
            // create RSA object from System.Security.Cryptography
            RSACryptoServiceProvider rsaObject = new RSACryptoServiceProvider(algoLength);
            // set RSA object with xml string
            rsaObject.FromXmlString(xmlString);
            byte[] result = null;

            try
            {
                result = rsaObject.SignData(byteInput, "SHA256");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        static byte[] GenerateChallenge()
        {
            byte[] bytes = new byte[128];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return bytes;
        }
        // verifying with RSA
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
    }
}
