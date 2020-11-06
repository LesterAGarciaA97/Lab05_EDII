using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace BibliotecaDeClases.Cifrados
{
    public class Route
    {
        private static string routeDirectory = Environment.CurrentDirectory;
        //Cifrado realizado no utilizado
        #region CifradoVertical
        //public static void CifradoVertical(IFormFile File, int Columns, int Rows)
        //{
        //    string NewName = Path.GetFileNameWithoutExtension(File.FileName);
        //    using (var reader = new BinaryReader(File.OpenReadStream()))
        //    {
        //        using (var streamWriter = new FileStream($"temp\\{NewName}.rt", FileMode.OpenOrCreate))
        //        {
        //            using (var writer = new BinaryWriter(streamWriter))
        //            {
        //                var bufferLength = Columns * Rows;
        //                var byteBuffer = new byte[bufferLength];

        //                while (reader.BaseStream.Position != reader.BaseStream.Length)
        //                {
        //                    var matriz = new byte[Columns, Rows];
        //                    byteBuffer = reader.ReadBytes(bufferLength);
        //                    var cont = 0;

        //                    for (int i = 0; i < Rows; i++)
        //                    {
        //                        for (int j = 0; j < Columns; j++)
        //                        {
        //                            if (cont < byteBuffer.Count())
        //                            {
        //                                matriz[j, i] = byteBuffer[cont];
        //                                cont++;
        //                            }
        //                        }
        //                    }

        //                    for (int i = 0; i < Columns; i++)
        //                    {
        //                        for (int j = 0; j < Rows; j++)
        //                        {
        //                            writer.Write(matriz[i, j]);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //public static void DecifradoVertical(CipherData info)
        //{

        //    string NewName = Path.GetFileNameWithoutExtension(info.File.FileName);
        //    using (var reader = new BinaryReader(info.File.OpenReadStream()))
        //    {
        //        using (var streamWriter = new FileStream($"temp\\{NewName}.txt", FileMode.OpenOrCreate))
        //        {
        //            using (var writer = new BinaryWriter(streamWriter))
        //            {
        //                var bufferLength = Columns *Rows;
        //                var byteBuffer = new byte[bufferLength];

        //                while (reader.BaseStream.Position != reader.BaseStream.Length)
        //                {
        //                    var matriz = new byte[Columns,Rows];
        //                    byteBuffer = reader.ReadBytes(bufferLength);
        //                    var cont = 0;

        //                    for (int i = 0; i < Columns; i++)
        //                    {
        //                        for (int j = 0; j <Rows; j++)
        //                        {
        //                            if (cont < byteBuffer.Count())
        //                            {
        //                                matriz[i, j] = byteBuffer[cont];
        //                                cont++;
        //                            }
        //                            else
        //                            {
        //                                matriz[i, j] = (byte)0;
        //                            }
        //                        }
        //                    }

        //                    for (int i = 0; i <Rows; i++)
        //                    {
        //                        for (int j = 0; j < Columns; j++)
        //                        {
        //                            if (matriz[j, i] != (byte)0)
        //                            {
        //                                writer.Write(matriz[j, i]);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion 

        public static void CifradoEspiral(IFormFile File, int Columns, int Rows)
        {
            string NewName = Path.GetFileNameWithoutExtension(File.FileName);
            using (var reader = new BinaryReader(File.OpenReadStream()))
            {
                using (var streamWriter = new FileStream($"temp\\{NewName}.rt", FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(streamWriter))
                    {
                        var bufferLength = Columns *Rows;
                        var byteBuffer = new byte[bufferLength];

                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            var Matriz = new byte[Columns,Rows];
                            byteBuffer = reader.ReadBytes(bufferLength);

                            var numVueltas = 0;
                            var posX = 0;
                            var posY = 0;
                            var Direccion = "abajo";

                            foreach (var caracter in byteBuffer)
                            {
                                if (Direccion == "abajo" && posY != Columns - 1 - numVueltas)
                                {
                                    Matriz[posY, posX] = caracter;
                                    posY++;
                                }
                                else if (Direccion == "abajo" && posY == Columns - 1 - numVueltas)
                                {
                                    Matriz[posY, posX] = caracter;
                                    posX++;
                                    Direccion = "derecha";
                                }
                                else if (Direccion == "derecha" && posX !=Rows - 1 - numVueltas)
                                {
                                    Matriz[posY, posX] = caracter;
                                    posX++;
                                }
                                else if (Direccion == "derecha" && posX ==Rows - 1 - numVueltas)
                                {
                                    Matriz[posY, posX] = caracter;
                                    posY--;
                                    Direccion = "arriba";
                                }
                                else if (Direccion == "arriba" && posY != numVueltas)
                                {
                                    Matriz[posY, posX] = caracter;
                                    posY--;
                                }
                                else if (Direccion == "arriba" && posY == numVueltas)
                                {
                                    Matriz[posY, posX] = caracter;
                                    numVueltas++;
                                    posX--;
                                    Direccion = "izquierda";
                                }
                                else if (Direccion == "izquierda" && posX != numVueltas)
                                {
                                    Matriz[posY, posX] = caracter;
                                    posX--;
                                }
                                else if (Direccion == "izquierda" && posX == numVueltas)
                                {
                                    Matriz[posY, posX] = caracter;
                                    posY++;
                                    Direccion = "abajo";
                                }
                            }

                            for (int i = 0; i < Columns; i++)
                            {
                                for (int j = 0; j <Rows; j++)
                                {
                                    writer.Write(Matriz[i, j]);
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void DecifradoEspiral(IFormFile File, int Columns, int Rows)
        {

            string NewName = Path.GetFileNameWithoutExtension(File.FileName);
            using (var reader = new BinaryReader(File.OpenReadStream()))
            {
                using (var streamWriter = new FileStream($"temp\\{NewName}.txt", FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(streamWriter))
                    {
                        var bufferLength = Columns *Rows;
                        var byteBuffer = new byte[bufferLength];

                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            var Matriz = new byte[Columns,Rows];
                            byteBuffer = reader.ReadBytes(bufferLength);
                            var cont = 0;

                            for (int i = 0; i < Columns; i++)
                            {
                                for (int j = 0; j <Rows; j++)
                                {
                                    if (cont < byteBuffer.Count())
                                    {
                                        Matriz[i, j] = byteBuffer[cont];
                                        cont++;
                                    }
                                    else
                                    {
                                        Matriz[i, j] = (byte)0;
                                    }
                                }
                            }

                            var numVueltas = 0;
                            var posX = 0;
                            var posY = 0;
                            var Direccion = "abajo";

                            for (int i = 0; i < bufferLength; i++)
                            {
                                if (Matriz[posY, posX] != 0)
                                {
                                    if (Direccion == "abajo" && posY != Columns - 1 - numVueltas)
                                    {
                                        writer.Write(Matriz[posY, posX]);
                                        posY++;
                                    }
                                    else if (Direccion == "abajo" && posY == Columns - 1 - numVueltas)
                                    {
                                        writer.Write(Matriz[posY, posX]);
                                        posX++;
                                        Direccion = "derecha";
                                    }
                                    else if (Direccion == "derecha" && posX !=Rows - 1 - numVueltas)
                                    {
                                        writer.Write(Matriz[posY, posX]);
                                        posX++;
                                    }
                                    else if (Direccion == "derecha" && posX ==Rows - 1 - numVueltas)
                                    {
                                        writer.Write(Matriz[posY, posX]);
                                        posY--;
                                        Direccion = "arriba";
                                    }
                                    else if (Direccion == "arriba" && posY != numVueltas)
                                    {
                                        writer.Write(Matriz[posY, posX]);
                                        posY--;
                                    }
                                    else if (Direccion == "arriba" && posY == numVueltas)
                                    {
                                        writer.Write(Matriz[posY, posX]);
                                        numVueltas++;
                                        posX--;
                                        Direccion = "izquierda";
                                    }
                                    else if (Direccion == "izquierda" && posX != numVueltas)
                                    {
                                        writer.Write(Matriz[posY, posX]);
                                        posX--;
                                    }
                                    else if (Direccion == "izquierda" && posX == numVueltas)
                                    {
                                        writer.Write(Matriz[posY, posX]);
                                        posY++;
                                        Direccion = "abajo";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}