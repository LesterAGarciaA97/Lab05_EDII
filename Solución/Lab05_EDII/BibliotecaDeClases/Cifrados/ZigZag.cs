using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BibliotecaDeClases.Cifrados
{
    public class ZigZag
    {
        private static string routeDirectory = Environment.CurrentDirectory;
        public static void Cifrar(IFormFile File, int Levels)
        {
            string NewName = Path.GetFileNameWithoutExtension(File.FileName);
            using (var reader = new BinaryReader(File.OpenReadStream()))
            {
                using (var streamWriter = new FileStream($"temp\\{NewName}.zz", FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(streamWriter))
                    {
                        var GrupoOlas = (2 * Levels) - 2;
                        var len = (float)reader.BaseStream.Length / (float)GrupoOlas;
                        var cantOlas = len % 1 <= 0.5 ? Math.Round(len) + 1 : Math.Round(len);
                        cantOlas = Convert.ToInt32(cantOlas);

                        var pos = 0;
                        var contNivel = 0;

                        var mensaje = new List<byte>[Levels];

                        for (int i = 0; i < Levels; i++)
                        {
                            mensaje[i] = new List<byte>();
                        }

                        var bufferLength = 100000;
                        var byteBuffer = new byte[bufferLength];

                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            byteBuffer = reader.ReadBytes(bufferLength);
                            foreach (var caracter in byteBuffer)
                            {
                                if (pos == 0 || pos % GrupoOlas == 0)
                                {
                                    mensaje[0].Add(caracter);
                                    contNivel = 0;
                                }
                                else if (pos % GrupoOlas == Levels - 1)
                                {
                                    mensaje[Levels - 1].Add(caracter);
                                    contNivel = Levels - 1;
                                }
                                else if (pos % GrupoOlas < Levels - 1)
                                {
                                    contNivel++;
                                    mensaje[contNivel].Add(caracter);
                                }
                                else if (pos % GrupoOlas > Levels - 1)
                                {
                                    contNivel--;
                                    mensaje[contNivel].Add(caracter);
                                }
                                pos++;
                            }
                        }

                        for (int i = 0; i < Levels; i++)
                        {
                            var cantIteracion = i == 0 || i == Levels - 1 ? cantOlas : cantOlas * 2;
                            var inicio = mensaje[i].Count();
                            for (int j = inicio; j < cantIteracion; j++)
                            {
                                mensaje[i].Add((byte)0);
                            }
                            writer.Write(mensaje[i].ToArray());
                        }
                    }
                }
            }
        }

        ///Decifrar
        public static void Decifrar(IFormFile File, int Levels)
        {

            string NewName = Path.GetFileNameWithoutExtension(File.FileName);
            using (var reader = new BinaryReader(File.OpenReadStream()))
            {
                using (var streamWriter = new FileStream($"temp\\{NewName}.txt", FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(streamWriter))
                    {
                        var GrupoOlas = (2 * Levels) - 2;
                        var cantOlas = Convert.ToInt32(reader.BaseStream.Length) / GrupoOlas;
                        var intermedios = (Convert.ToInt32(reader.BaseStream.Length) - (2 * cantOlas)) / (Levels - 2);

                        var pos = 0;
                        var contNivel = 0;
                        var contIntermedio = 0;

                        var mensaje = new Queue<byte>[Levels];

                        for (int i = 0; i < Levels; i++)
                        {
                            mensaje[i] = new Queue<byte>();
                        }

                        var bufferLength = 100000;
                        var byteBuffer = new byte[bufferLength];

                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            byteBuffer = reader.ReadBytes(bufferLength);
                            foreach (var caracter in byteBuffer)
                            {
                                if (contNivel == Levels - 1)
                                {
                                    mensaje[contNivel].Enqueue(caracter);
                                }
                                else
                                {
                                    if (pos < cantOlas)
                                    {
                                        mensaje[0].Enqueue(caracter);
                                    }
                                    else if (pos == cantOlas)
                                    {
                                        contNivel++;
                                        mensaje[contNivel].Enqueue(caracter);
                                        contIntermedio = 1;
                                    }
                                    else if (contIntermedio < intermedios)
                                    {
                                        mensaje[contNivel].Enqueue(caracter);
                                        contIntermedio++;
                                    }
                                    else
                                    {
                                        contNivel++;
                                        mensaje[contNivel].Enqueue(caracter);
                                        contIntermedio = 1;
                                    }
                                    pos++;
                                }
                            }
                        }

                        contNivel = 0;
                        var direccion = true;
                        //True es hacia abajo
                        //False es hacia arriba

                        while (mensaje[1].Count() != 0 || (Levels == 2 && mensaje[1].Count() != 0))
                        {
                            if (contNivel == 0)
                            {
                                writer.Write(mensaje[contNivel].Dequeue());
                                contNivel = 1;
                                direccion = true;
                            }
                            else if (contNivel < Levels - 1 && direccion)
                            {
                                writer.Write(mensaje[contNivel].Dequeue());
                                contNivel++;
                            }
                            else if (contNivel > 0 && !direccion)
                            {
                                writer.Write(mensaje[contNivel].Dequeue());
                                contNivel--;
                            }
                            else if (contNivel == Levels - 1)
                            {
                                writer.Write(mensaje[contNivel].Dequeue());
                                contNivel = Levels - 2;
                                direccion = false;
                            }
                        }
                    }
                }
            }
        }

    }
}