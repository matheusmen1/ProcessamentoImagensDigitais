using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ProcessamentoImagens
{
    class Filtros
    {
        //sem acesso direto a memoria
        public static void convert_to_gray(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);

                    //nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);

                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        //sem acesso direito a memoria
        public static void negativo(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;

                    //nova cor
                    Color newcolor = Color.FromArgb(255 - r, 255 - g, 255 - b);

                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        //com acesso direto a memória
        public static void convert_to_grayDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;
            Int32 gs;

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();

                int r, g, b;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b = *(src++); //está armazenado dessa forma: b g r 
                        g = *(src++);
                        r = *(src++);
                        gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);
                        *(dst++) = (byte)gs;
                        *(dst++) = (byte)gs;
                        *(dst++) = (byte)gs;
                    }
                    src += padding;
                    dst += padding;
                }
            }
            //unlock imagem origem
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }

        //com acesso direito a memoria
        public static void negativoDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;

            //lock dados bitmap origem 
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src1 = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();

                int r, g, b;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b = *(src1++); //está armazenado dessa forma: b g r 
                        g = *(src1++);
                        r = *(src1++);

                        *(dst++) = (byte)(255 - b);
                        *(dst++) = (byte)(255 - g);
                        *(dst++) = (byte)(255 - r);
                    }
                    src1 += padding;
                    dst += padding;
                }
            }
            //unlock imagem origem 
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }
        public static void espelharVertical (Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width; // pega a largura da imagem em inteiros
            int height = imageBitmapSrc.Height; // pega a altura da imagem em inteiros
            Color cor1, cor2;
            

            for (int i = 0; i < height/2; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //obtendo a cor do pixel
                    cor1 = imageBitmapSrc.GetPixel(j, i);
                    cor2 = imageBitmapSrc.GetPixel(j, height - i - 1);
                    // coluna  e linha
                    
                        
                    imageBitmapDest.SetPixel(j, i, cor2);
                    imageBitmapDest.SetPixel(j, height - i - 1, cor1);
                }
            }
           
        }
        public static void espelharHorizontal(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width; // pega a largura da imagem em inteiros
            int height = imageBitmapSrc.Height; // pega a altura da imagem em inteiros
            Color cor1, cor2;

            for (int j = 0; j < width / 2; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    //obtendo a cor do pixel
                    cor1 = imageBitmapSrc.GetPixel(j, i);
                    cor2 = imageBitmapSrc.GetPixel(width - j - 1, i);
                    // coluna  e linha


                    imageBitmapDest.SetPixel(j, i, cor2);
                    imageBitmapDest.SetPixel(width - j- 1, i, cor1);
                }
            }

        }

        public static void afinamentoDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width; // pega a largura da imagem em inteiros
            int height = imageBitmapSrc.Height; // pega a altura da imagem em inteiros
            int pixelSize = 3;

            //lock dados bitmap origem 
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            int[] pontosRemovidos = new int[(width * height) * 2];
            int TL = 0;
            unsafe
            {
                
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();
                byte* aux, srcAux, dstAux;
                int r1, r2, r3, r4, r5, r6, r7, r8, r9;
                int b1, b2, b3, b4, b5, b6, b7, b8, b9;
                int g1, g2, g3, g4, g5, g6, g7, g8, g9;
                int conectividade, preto;
                int flag = 0;
                while(flag != 1)
                {
                    for (int y = 1; y < height - 1; y++)
                    {
                        for (int x = 1; x < width - 1; x++)
                        {

                            srcAux = src + y * bitmapDataSrc.Stride + x * 3;
                            b1 = *(srcAux++); //está armazenado dessa forma: b g r  // peguei o rgb do pixel (i, j)
                            g1 = *(srcAux++);
                            r1 = *(srcAux++);

                            conectividade = 0;
                            preto = 0;
                            if (r1 < 127 && g1 < 127 && b1 < 127) // achei um pixel preto
                            {
                                // agora preciso deslocar meu ponteiro para o bgr dos meus 8 vizinhos

                                aux = src + y * bitmapDataSrc.Stride + (x + 1) * 3;
                                b4 = *(aux++);
                                g4 = *(aux++);
                                r4 = *(aux++);

                                aux = src + y * bitmapDataSrc.Stride + (x - 1) * 3;
                                b8 = *(aux++);
                                g8 = *(aux++);
                                r8 = *(aux++);

                                aux = src + (y - 1) * bitmapDataSrc.Stride + (x - 1) * 3;
                                b9 = *(aux++);
                                g9 = *(aux++);
                                r9 = *(aux++);

                                aux = src + (y - 1) * bitmapDataSrc.Stride + x * 3;
                                b2 = *(aux++);
                                g2 = *(aux++);
                                r2 = *(aux++);

                                aux = src + (y - 1) * bitmapDataSrc.Stride + (x + 1) * 3;
                                b3 = *(aux++);
                                g3 = *(aux++);
                                r3 = *(aux++);

                                aux = src + (y + 1) * bitmapDataSrc.Stride + (x - 1) * 3;
                                b7 = *(aux++);
                                g7 = *(aux++);
                                r7 = *(aux++);

                                aux = src + (y + 1) * bitmapDataSrc.Stride + x * 3;
                                b6 = *(aux++);
                                g6 = *(aux++);
                                r6 = *(aux++);

                                aux = src + (y + 1) * bitmapDataSrc.Stride + (x + 1) * 3;
                                b5 = *(aux++);
                                g5 = *(aux++);
                                r5 = *(aux++);

                                if ((r2 >= 127 && g2 >= 127 && b2 >= 127) && (r3 < 127 && g3 < 127 && b3 < 127)) // branco para preto
                                    conectividade++;
                                if ((r3 >= 127 && g3 >= 127 && b3 >= 127) && (r4 < 127 && g4 < 127 && b4 < 127))
                                    conectividade++;
                                if ((r4 >= 127 && g4 >= 127 && b4 >= 127) && (r5 < 127 && g5 < 127 && b5 < 127))
                                    conectividade++;
                                if ((r5 >= 127 && g5 >= 127 && b5 >= 127) && (r6 < 127 && g6 < 127 && b6 < 127))
                                    conectividade++;
                                if ((r6 >= 127 && g6 >= 127 && b6 >= 127) && (r7 < 127 && g7 < 127 && b7 < 127))
                                    conectividade++;
                                if ((r7 >= 127 && g7 >= 127 && b7 >= 127) && (r8 < 127 && g8 < 127 && b8 < 127))
                                    conectividade++;
                                if ((r8 >= 127 && g8 >= 127 && b8 >= 127) && (r9 < 127 && g9 < 127 && b9 < 127))
                                    conectividade++;
                                if ((r9 >= 127 && g9 >= 127 && b9 >= 127) && (r2 < 127 && g2 < 127 && b2 < 127))
                                    conectividade++;

                                if (conectividade == 1)
                                {

                                    if (r9 < 127 && g9 < 127 && b9 < 127)
                                        preto++;
                                    if (r8 < 127 && g8 < 127 && b8 < 127)
                                        preto++;
                                    if (r7 < 127 && g7 < 127 && b7 < 127)
                                        preto++;
                                    if (r6 < 127 && g6 < 127 && b6 < 127)
                                        preto++;
                                    if (r5 < 127 && g5 < 127 && b5 < 127)
                                        preto++;
                                    if (r4 < 127 && g4 < 127 && b4 < 127)
                                        preto++;
                                    if (r3 < 127 && g3 < 127 && b3 < 127)
                                        preto++;
                                    if (r2 < 127 && g2 < 127 && b2 < 127)
                                        preto++;

                                    if (preto > 1 && preto < 7)
                                    {
                                        if ((r2 >= 127 && g2 >= 127 && b2 >= 127) || (r4 >= 127 && g4 >= 127 && b4 >= 127) || (r8 >= 127 && g8 >= 127 && b8 >= 127))
                                        {
                                            if ((r2 >= 127 && g2 >= 127 && b2 >= 127) || (r6 >= 127 && g6 >= 127 && b6 >= 127) || (r8 >= 127 && g8 >= 127 && b8 >= 127))
                                            {

                                                pontosRemovidos[TL++] = y; //linha
                                                pontosRemovidos[TL++] = x; //coluna
                                            }
                                        }
                                    }
                                }

                            }

                        }
                        //src += padding;
                        //dst += padding;
                    }
                    // deletar apos a primeira iteracao
                    int i = 0;
                    while (i < TL)
                    {
                        int y = pontosRemovidos[i++];
                        int x = pontosRemovidos[i++];
                        dstAux = dst + y * bitmapDataDst.Stride + x * 3;
                        srcAux = src + y * bitmapDataSrc.Stride + x * 3;
                        *(dstAux++) = 255;
                        *(dstAux++) = 255;
                        *(dstAux++) = 255;
                        *(srcAux++) = 255;
                        *(srcAux++) = 255;
                        *(srcAux++) = 255;
                    }
                    // segunda iteracao
                    TL = 0;
                    for (int y = 1; y < height - 1; y++)
                    {
                        for (int x = 1; x < width - 1; x++)
                        {
                            srcAux = src + y * bitmapDataSrc.Stride + x * 3;
                            b1 = *(srcAux++); //está armazenado dessa forma: b g r  // peguei o rgb do pixel (i, j)
                            g1 = *(srcAux++);
                            r1 = *(srcAux++);


                            conectividade = 0;
                            preto = 0;
                            if (r1 < 127 && g1 < 127 && b1 < 127) // achei um pixel preto
                            {
                                // agora preciso deslocar meu ponteiro para o bgr dos meus 8 vizinhos

                                aux = src + y * bitmapDataSrc.Stride + (x + 1) * 3;
                                b4 = *(aux++);
                                g4 = *(aux++);
                                r4 = *(aux++);

                                aux = src + y * bitmapDataSrc.Stride + (x - 1) * 3;
                                b8 = *(aux++);
                                g8 = *(aux++);
                                r8 = *(aux++);

                                aux = src + (y - 1) * bitmapDataSrc.Stride + (x - 1) * 3;
                                b9 = *(aux++);
                                g9 = *(aux++);
                                r9 = *(aux++);

                                aux = src + (y - 1) * bitmapDataSrc.Stride + x * 3;
                                b2 = *(aux++);
                                g2 = *(aux++);
                                r2 = *(aux++);

                                aux = src + (y - 1) * bitmapDataSrc.Stride + (x + 1) * 3;
                                b3 = *(aux++);
                                g3 = *(aux++);
                                r3 = *(aux++);

                                aux = src + (y + 1) * bitmapDataSrc.Stride + (x - 1) * 3;
                                b7 = *(aux++);
                                g7 = *(aux++);
                                r7 = *(aux++);

                                aux = src + (y + 1) * bitmapDataSrc.Stride + x * 3;
                                b6 = *(aux++);
                                g6 = *(aux++);
                                r6 = *(aux++);

                                aux = src + (y + 1) * bitmapDataSrc.Stride + (x + 1) * 3;
                                b5 = *(aux++);
                                g5 = *(aux++);
                                r5 = *(aux++);

                                if ((r2 >= 127 && g2 >= 127 && b2 >= 127) && (r3 < 127 && g3 < 127 && b3 < 127)) // branco para preto
                                    conectividade++;
                                if ((r3 >= 127 && g3 >= 127 && b3 >= 127) && (r4 < 127 && g4 < 127 && b4 < 127))
                                    conectividade++;
                                if ((r4 >= 127 && g4 >= 127 && b4 >= 127) && (r5 < 127 && g5 < 127 && b5 < 127))
                                    conectividade++;
                                if ((r5 >= 127 && g5 >= 127 && b5 >= 127) && (r6 < 127 && g6 < 127 && b6 < 127))
                                    conectividade++;
                                if ((r6 >= 127 && g6 >= 127 && b6 >= 127) && (r7 < 127 && g7 < 127 && b7 < 127))
                                    conectividade++;
                                if ((r7 >= 127 && g7 >= 127 && b7 >= 127) && (r8 < 127 && g8 < 127 && b8 < 127))
                                    conectividade++;
                                if ((r8 >= 127 && g8 >= 127 && b8 >= 127) && (r9 < 127 && g9 < 127 && b9 < 127))
                                    conectividade++;
                                if ((r9 >= 127 && g9 >= 127 && b9 >= 127) && (r2 < 127 && g2 < 127 && b2 < 127))
                                    conectividade++;

                                if (conectividade == 1)
                                {
                                    if (r9 < 127 && g9 < 127 && b9 < 127)
                                        preto++;
                                    if (r8 < 127 && g8 < 127 && b8 < 127)
                                        preto++;
                                    if (r7 < 127 && g7 < 127 && b7 < 127)
                                        preto++;
                                    if (r6 < 127 && g6 < 127 && b6 < 127)
                                        preto++;
                                    if (r5 < 127 && g5 < 127 && b5 < 127)
                                        preto++;
                                    if (r4 < 127 && g4 < 127 && b4 < 127)
                                        preto++;
                                    if (r3 < 127 && g3 < 127 && b3 < 127)
                                        preto++;
                                    if (r2 < 127 && g2 < 127 && b2 < 127)
                                        preto++;

                                    if (preto > 1 && preto < 7)
                                    {
                                        if ((r2 >= 127 && g2 >= 127 && b2 >= 127) || (r4 >= 127 && g4 >= 127 && b4 >= 127) || (r6 >= 127 && g6 >= 127 && b6 >= 127))
                                        {
                                            if ((r4 >= 127 && g4 >= 127 && b4 >= 127) || (r6 >= 127 && g6 >= 127 && b6 >= 127) || (r8 >= 127 && g8 >= 127 && b8 >= 127))
                                            {

                                                pontosRemovidos[TL++] = y; //linha
                                                pontosRemovidos[TL++] = x; //coluna
                                            }
                                        }

                                    }
                                }

                            }

                        }
                        //src1 += padding;
                        //dst += padding;
                    }

                    if (TL > 0)
                    {
                        i = 0;
                        while (i < TL)
                        {
                            int y = pontosRemovidos[i++];
                            int x = pontosRemovidos[i++];
                            dstAux = dst + y * bitmapDataDst.Stride + x * 3;
                            srcAux = src + y * bitmapDataSrc.Stride + x * 3;
                            *(dstAux++) = 255;
                            *(dstAux++) = 255;
                            *(dstAux++) = 255;
                            *(srcAux++) = 255;
                            *(srcAux++) = 255;
                            *(srcAux++) = 255;

                        }
                    }
                    else
                        flag = 1;
                }
            }
                
            //unlock imagem origem 
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }
    }
}
