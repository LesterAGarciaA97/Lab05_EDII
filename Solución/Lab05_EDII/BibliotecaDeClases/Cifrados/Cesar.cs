using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

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

        public static void Cifrar(IFormFile File, string Word )
        {
            ObtenerDic(Word, 1);
            string NewName = Path.GetFileNameWithoutExtension(File.FileName);

            using (var reader = new BinaryReader(File.OpenReadStream()))
            {
                using (var streamWriter = new FileStream($"Temp\\{NewName}.csr", FileMode.OpenOrCreate))
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
                    reader.Close();
                }
            }
        }

        public static void Decifrar(IFormFile File, string Word)
        {
            ObtenerDic(Word, 2);
            string NewName = Path.GetFileNameWithoutExtension(File.FileName);
            using (var reader = new BinaryReader(File.OpenReadStream()))
            {
                using (var streamWriter = new FileStream($"Temp\\{NewName}.txt", FileMode.OpenOrCreate))
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