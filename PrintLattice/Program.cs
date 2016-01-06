using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace PrintLattice
{
				class Program
				{
								static void Main(string[] args)
								{
												Console.Title = "Lattice Print";
												Console.WindowWidth = 140;
												Console.BufferWidth = 140;
												Console.BufferHeight = 300;
												string msg = " ";
												ConsoleColorWriteLine(msg, ConsoleColor.Green);
												bool blnContinue = true;
												while (blnContinue)
												{
																msg = "将文字转为点阵图，请按 1";
																ConsoleColorWriteLine(msg, ConsoleColor.Green);
																msg = "将图片文件转为点阵图，请按 2";
																ConsoleColorWriteLine(msg, ConsoleColor.Green);
																string strSelect = Console.ReadKey().KeyChar.ToString();
																if (strSelect == "1")
																{
																				msg = " ";
																				ConsoleColorWriteLine(msg, ConsoleColor.Green);
																				msg = "请输入文字：";
																				ConsoleColorWriteLine(msg, ConsoleColor.Green);
																				string strWord = Console.ReadLine();
																				PrintLatticeChar(strWord);
																				Console.ReadKey();
																				blnContinue = false;
																}
																else if (strSelect == "2")
																{
																				msg = " ";
																				ConsoleColorWriteLine(msg, ConsoleColor.Green);
																				msg = @"请输入图片文件路径： e.g. C:\demo.JPG";
																				ConsoleColorWriteLine(msg, ConsoleColor.Green);
																				string fileName = Console.ReadLine();
																				Image i = Bitmap.FromFile(fileName);
																				Bitmap bm = new Bitmap(i, 120, 120);
																				Thresholding(ToGray(bm, 1));
																				//Bitmap bmp = ToGray(new Bitmap(i),0);
																				//Bitmap bm = new Bitmap(bmp, 50, 50);
																				//bm.Save("c:/1.jpg", ImageFormat.Jpeg);
																				//Graphics g = Graphics.FromImage(i);
																				//g.InterpolationMode = InterpolationMode.HighQualityBicubic;
																				//g.DrawImage(bm, new Rectangle(0, 0, 80, 80), new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
																				//g.Dispose();
																				PrintBmpLatticeChar(bm);
																				Console.ReadKey();
																				blnContinue = false;
																}
																else
																{
																				msg = " ";
																				ConsoleColorWriteLine(msg, ConsoleColor.Red);
																				msg = "选择错误，请重新输入！";
																				ConsoleColorWriteLine(msg, ConsoleColor.Red);
																				blnContinue = true;
																}
												}												
								}
								static int[,] GetLatticeArray(string s)
								{
												FontStyle style = FontStyle.Regular;
												string familyName = "Veranda";
												float emSize = 14;
												Font f = new Font(familyName, emSize, style);

												int width = 20;
												int height = 20;
												Bitmap bm = new Bitmap(width, height);

												Graphics g = Graphics.FromImage(bm);

												Brush b = new SolidBrush(Color.Black);
												PointF pf = new PointF(-3, -4);

												g.DrawString(s, f, b, pf);
												g.Flush();
												g.Dispose();

												//bm.Save("test.bmp");

												int[,] a = new int[20, 20];
												for (int i = 0; i < bm.Width; i++)
												{
																for (int j = 0; j < bm.Height; j++)
																{
																				Color c = bm.GetPixel(j, i);
																				if (c.Name == "0")
																				{
																								a[j, i] = 0;
																				}
																				else
																				{
																								a[j, i] = 1;
																				}
																}
												}
												bm.Dispose();
												return a;
								}
								static void PrintLatticeChar(string s)
								{
												int x = Console.CursorLeft;
												int y = Console.CursorTop;
												foreach (char c in s.ToCharArray())
												{
																int[,] a = GetLatticeArray(c.ToString());
																int charWidth = a.GetLength(0);
																for (int i = 0; i < charWidth; i++)
																{
																				for (int j = 0; j < charWidth; j++)
																				{
																								if (a[j, i] == 1)
																								{
																												//Console.Write("$");
																												var msg = "$";
																												ConsoleColorWrite(msg, ConsoleColor.Green);
																								}
																								else
																								{
																												//Console.Write(" ");
																												var msg = " ";
																												ConsoleColorWrite(msg, ConsoleColor.Green);
																								}
																				}
																				x = Console.CursorLeft - charWidth;
																				if (x <= 0)
																				{
																								x = 1;
																				}
																				Console.CursorLeft = x;
																				Console.CursorTop++;
																}
																if ((Console.CursorLeft + charWidth) > Console.WindowWidth)
																{
																				Console.CursorTop += charWidth;
																}
																else
																{
																				Console.CursorLeft += charWidth;
																				Console.CursorTop -= charWidth;
																}
												}
								}
								static int[,] GetBmpLatticeArray(Bitmap bm, int w, int h)
								{
												int width = bm.Width;
												int height = bm.Height;
												int[,] a = new int[w, h];
												for (int j = 0; j < height; j++)
												{
																for (int i = 0; i < width; i++)
																{
																				Color c = bm.GetPixel(i, j);
																				if (c.Name == "ffffffff")
																				{
																								a[i, j] = 1;
																				}
																				else
																				{
																								a[i, j] = 0;
																				}
																}
												}
												bm.Dispose();
												return a;
								}
								static void PrintBmpLatticeChar(Bitmap bm)
								{
												int x = Console.CursorLeft;
												int y = Console.CursorTop;
												int bmWidth = bm.Width;
												int bmHeight = bm.Height;
												int[,] a = GetBmpLatticeArray(bm, bmWidth, bmHeight);
												for (int i = 0; i < bmHeight; i++)
												{
																Console.Write(i.ToString().PadLeft(3, '0') + " ");
																for (int j = 0; j < bmWidth; j++)
																{
																				if (a[j, i] == 1)
																				{
																								Console.Write("$");
																								//StreamWriter f = new StreamWriter(@"c:\hello.txt", true);
																								//f.Write("$");
																								//f.Close();
																				}
																				else
																				{
																								Console.Write(" ");
																								//StreamWriter f = new StreamWriter(@"c:\hello.txt", true);
																								//f.Write(" ");
																								//f.Close();
																				}
																}
																x = Console.CursorLeft - bmWidth - 4;
																if (x <= 0)
																{
																				x = 1;
																}
																Console.CursorLeft = x;
																Console.CursorTop++;
																//StreamWriter fa = new StreamWriter(@"c:\hello.txt", true);
																//fa.Write("\n");
																//fa.Close();
												}
								}
								//
								/// <summary>
								/// 变成黑白图
								/// </summary>
								/// <param name="bmp">原始图</param>
								/// <param name="mode">模式。0:加权平均  1:算数平均</param>
								/// <returns></returns>
								static Bitmap ToGray(Bitmap bmp, int mode)
								{
												if (bmp == null)
												{
																return null;
												}
												int w = bmp.Width;
												int h = bmp.Height;
												try
												{
																byte newColor = 0;
																BitmapData srcData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
																unsafe
																{
																				byte* p = (byte*)srcData.Scan0.ToPointer();
																				for (int y = 0; y < h; y++)
																				{
																								for (int x = 0; x < w; x++)
																								{

																												if (mode == 0)　// 加权平均
																												{
																																newColor = (byte)((float)p[0] * 0.114f + (float)p[1] * 0.587f + (float)p[2] * 0.299f);
																												}
																												else　　　　// 算数平均
																												{
																																newColor = (byte)((float)(p[0] + p[1] + p[2]) / 3.0f);
																												}
																												p[0] = newColor;
																												p[1] = newColor;
																												p[2] = newColor;

																												p += 3;
																								}
																								p += srcData.Stride - w * 3;
																				}
																				bmp.UnlockBits(srcData);
																				return bmp;
																}
												}
												catch
												{
																return null;
												}
								}
								static void ToGrey2(Bitmap img1)
								{
												for (int i = 0; i < img1.Width; i++)
												{
																for (int j = 0; j < img1.Height; j++)
																{
																				Color pixelColor = img1.GetPixel(i, j);
																				//计算灰度值
																				int grey = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
																				Color newColor = Color.FromArgb(grey, grey, grey);
																				img1.SetPixel(i, j, newColor);
																}
												}
								}
								static void Thresholding(Bitmap img1)
								{
												int[] histogram = new int[256];
												int minGrayValue = 255, maxGrayValue = 0;
												//求取直方图
												for (int i = 0; i < img1.Width; i++)
												{
																for (int j = 0; j < img1.Height; j++)
																{
																				Color pixelColor = img1.GetPixel(i, j);
																				histogram[pixelColor.R]++;
																				if (pixelColor.R > maxGrayValue) maxGrayValue = pixelColor.R;
																				if (pixelColor.R < minGrayValue) minGrayValue = pixelColor.R;
																}
												}
												//迭代计算阀值
												int threshold = -1;
												int newThreshold = (minGrayValue + maxGrayValue) / 2;
												for (int iterationTimes = 0; threshold != newThreshold && iterationTimes < 100; iterationTimes++)
												{
																threshold = newThreshold;
																int lP1 = 0;
																int lP2 = 0;
																int lS1 = 0;
																int lS2 = 0;
																//求两个区域的灰度的平均值
																for (int i = minGrayValue; i < threshold; i++)
																{
																				lP1 += histogram[i] * i;
																				lS1 += histogram[i];
																}
																int mean1GrayValue = (lP1 / lS1);
																for (int i = threshold + 1; i < maxGrayValue; i++)
																{
																				lP2 += histogram[i] * i;
																				lS2 += histogram[i];
																}
																int mean2GrayValue = (lP2 / lS2);
																newThreshold = (mean1GrayValue + mean2GrayValue) / 2;
												}
												//计算二值化
												for (int i = 0; i < img1.Width; i++)
												{
																for (int j = 0; j < img1.Height; j++)
																{
																				Color pixelColor = img1.GetPixel(i, j);
																				if (pixelColor.R > threshold) img1.SetPixel(i, j, Color.FromArgb(255, 255, 255));
																				else img1.SetPixel(i, j, Color.FromArgb(0, 0, 0));
																}
												}
								}
								//
								static void ConsoleColorWrite(string msg, ConsoleColor cc)
								{
												Console.BackgroundColor = ConsoleColor.Black;
												Console.ForegroundColor = cc;
												Console.Write(msg);
												Console.ResetColor();
								}
								static void ConsoleColorWriteLine(string msg, ConsoleColor cc)
								{
												Console.BackgroundColor = ConsoleColor.Black;
												Console.ForegroundColor = cc;
												Console.WriteLine(EncodingString(msg, Encoding.UTF8));
												Console.ResetColor();
								}
								static string EncodingString(string msg, Encoding type)
								{
												byte[] srcBytes = Encoding.Default.GetBytes(msg);
												byte[] bytes = Encoding.Convert(Encoding.Default, type, srcBytes);
												return type.GetString(bytes);
								}
				}
}
