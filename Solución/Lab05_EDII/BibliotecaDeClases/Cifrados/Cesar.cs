using BibliotecaDeClases.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BibliotecaDeClases.Cifrados
{
    public class Cesar
    {

        private static Dictionary<int, int> DirAlpha = new Dictionary<int, int>();
        private static string routeDirectory = Environment.CurrentDirectory;

        static void ObtenerDic(string key, int opc)
        {
            DirAlpha = new Dictionary<int, int>();
            key = key.ToUpper();
            var CountOriginal = 65;
            var CountNew = 65;

            if (opc == 1)
            {
                do
                {
                    if (key.Length > 0)
                    {
                        if (!DirAlpha.ContainsValue(key[0]))
                        {
                            DirAlpha.Add(CountOriginal, key[0]);
                            CountOriginal++;
                        }
                        key = key.Substring(1, key.Length - 1);
                    }
                    else
                    {
                        if (!DirAlpha.ContainsValue(CountNew))
                        {
                            DirAlpha.Add(CountOriginal, CountNew);
                            CountOriginal++;
                        }
                        CountNew++;
                    }
                } while (CountOriginal < 91);
            }
            else
            {
                do
                {
                    if (key.Length > 0)
                    {
                        if (!DirAlpha.ContainsKey(key[0]))
                        {
                            DirAlpha.Add(key[0], CountOriginal);
                            CountOriginal++;
                        }
                        key = key.Substring(1, key.Length - 1);
                    }
                    else
                    {
                        if (!DirAlpha.ContainsKey(CountNew))
                        {
                            DirAlpha.Add(CountNew, CountOriginal);
                            CountOriginal++;
                        }
                        CountNew++;
                    }
                } while (CountOriginal < 91);
            }
        }

        public static void Cifrar(CipherData info)
        {
            ObtenerDic(info.Key[0], 1);
            string NewName = Path.GetFileNameWithoutExtension(info.File.FileName);

            using (var reader = new BinaryReader(info.File.OpenReadStream()))
            {
                using (var streamWriter = new FileStream($"CifradoCesar\\{NewName}.csr", FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(streamWriter))
                    {
                        var bufferLength = 10000;
                        var byteBuffer = new byte[bufferLength];
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            byteBuffer = reader.ReadBytes(bufferLength);

                            foreach (var caracter in byteBuffer)
                            {
                                var actual = Convert.ToInt32(caracter);
                                if (actual >= 65 && actual <= 90)
                                {
                                    writer.Write((byte)DirAlpha[actual]);
                                }
                                else if (actual >= 97 && actual <= 122)
                                {
                                    writer.Write((byte)(DirAlpha[actual - 32] + 32));
                                }
                                else
                                {
                                    writer.Write(caracter);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void Decifrar(CipherData info)
        {
            ObtenerDic(info.Key[0], 2);
            string NewName = Path.GetFileNameWithoutExtension(info.File.FileName);
            using (var reader = new BinaryReader(info.File.OpenReadStream()))
            {
                using (var streamWriter = new FileStream($"DescrifradoCesar\\{NewName}.txt", FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(streamWriter))
                    {
                        var bufferLength = 10000;
                        var byteBuffer = new byte[bufferLength];
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            byteBuffer = reader.ReadBytes(bufferLength);

                            foreach (var caracter in byteBuffer)
                            {
                                var actual = Convert.ToInt32(caracter);
                                if (actual >= 65 && actual <= 90)
                                {
                                    writer.Write((byte)DirAlpha[actual]);
                                }
                                else if (actual >= 97 && actual <= 122)
                                {
                                    writer.Write((byte)(DirAlpha[actual - 32] + 32));
                                }
                                else
                                {
                                    writer.Write(caracter);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
