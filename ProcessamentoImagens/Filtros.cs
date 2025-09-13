using System;
using System.Drawing;
using System.Drawing.Imaging;

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
        public static void espelharVertical(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width; // pega a largura da imagem em inteiros
            int height = imageBitmapSrc.Height; // pega a altura da imagem em inteiros
            Color cor1, cor2;


            for (int i = 0; i < height / 2; i++)
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
                    imageBitmapDest.SetPixel(width - j - 1, i, cor1);
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
                byte* aux, srcAux, dstAux, aux1, aux2;

                for (int x = 0; x < imageBitmapSrc.Height; x++)
                {
                    for (int y = 0; y < imageBitmapSrc.Width; y++)
                    {
                        aux1 = src + x * bitmapDataSrc.Stride + y * pixelSize;
                        aux2 = dst + x * bitmapDataDst.Stride + y * pixelSize;
                        int media = (aux1[0] + aux1[1] + aux1[2]) / 3;
                        if (media < 128)
                        {
                            aux1[0] = 0;
                            aux1[1] = 0;
                            aux1[2] = 0;
                            aux2[0] = 0;
                            aux2[1] = 0;
                            aux2[2] = 0;
                        }
                        else
                        {
                            aux1[0] = 255;
                            aux1[1] = 255;
                            aux1[2] = 255;
                            aux2[0] = 255;
                            aux2[1] = 255;
                            aux2[2] = 255;
                        }

                    }
                }
                int r1, r2, r3, r4, r5, r6, r7, r8, r9;
                int b1, b2, b3, b4, b5, b6, b7, b8, b9;
                int g1, g2, g3, g4, g5, g6, g7, g8, g9;
                int conectividade, preto;
                int flag = 0;
                while (flag != 1)
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

        public static void contourFollowingDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
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
            unsafe
            {

                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();
                byte* srcAux, dstAux, aux, aux1;

                for (int y = 0; y < imageBitmapDest.Height; y++)
                {
                    for (int x = 0; x < imageBitmapDest.Width; x++)
                    {
                        aux1 = dst + y * bitmapDataDst.Stride + x * pixelSize;
                        aux1[0] = 255;
                        aux1[1] = 255;
                        aux1[2] = 255;
                    }
                }

                int pR, r0 = -1, r1 = -1, r2 = -1, r3 = -1, r4 = -1, r5 = -1, r6 = -1, r7 = -1;
                int pG, g0 = -1, g1 = -1, g2 = -1, g3 = -1, g4 = -1, g5 = -1, g6 = -1, g7 = -1;
                int pB, b0 = -1, b1 = -1, b2 = -1, b3 = -1, b4 = -1, b5 = -1, b6 = -1, b7 = -1;
                int direcao, proximaDirecao;
                int cordX, cordY, inicioY, inicioX, i, flag;
                int[] coordenadas = new int[(width * height) * 2];
                int TL = 0;
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        srcAux = src + y * bitmapDataSrc.Stride + x * pixelSize; // desloco o ponteiro para meu y e x atual
                        pB = *(srcAux++);
                        pG = *(srcAux++);
                        pR = *(srcAux++);

                        aux = src + y * bitmapDataSrc.Stride + (x + 1) * pixelSize; // pixel à direita
                        b0 = (*aux++);
                        g0 = (*aux++);
                        r0 = (*aux++);
                        if ((pR >= 127 && pG >= 127 && pB >= 127) && (r0 < 127 && g0 < 127 && b0 < 127)) // pixel preto à direita do branco
                        {
                            // primeiro elemento do contorno  
                            direcao = 4;

                            inicioY = y;
                            inicioX = x + 1;
                            cordX = inicioX;
                            cordY = inicioY;

                            do
                            {

                                if (cordX + 1 < width) // validação para não acessar lixo
                                {
                                    aux = src + cordY * bitmapDataSrc.Stride + (cordX + 1) * pixelSize;
                                    b0 = (*aux++);
                                    g0 = (*aux++);
                                    r0 = (*aux++);
                                }

                                if (cordX + 1 < width && cordY - 1 >= 0)
                                {
                                    aux = src + (cordY - 1) * bitmapDataSrc.Stride + (cordX + 1) * pixelSize;
                                    b1 = (*aux++);
                                    g1 = (*aux++);
                                    r1 = (*aux++);
                                }

                                if (cordY - 1 >= 0)
                                {
                                    aux = src + (cordY - 1) * bitmapDataSrc.Stride + cordX * pixelSize;
                                    b2 = (*aux++);
                                    g2 = (*aux++);
                                    r2 = (*aux++);
                                }

                                if (cordY - 1 >= 0 && cordX - 1 >= 0)
                                {
                                    aux = src + (cordY - 1) * bitmapDataSrc.Stride + (cordX - 1) * pixelSize;
                                    b3 = (*aux++);
                                    g3 = (*aux++);
                                    r3 = (*aux++);
                                }

                                if (cordX - 1 >= 0)
                                {
                                    aux = src + cordY * bitmapDataSrc.Stride + (cordX - 1) * pixelSize;
                                    b4 = (*aux++);
                                    g4 = (*aux++);
                                    r4 = (*aux++);
                                }

                                if (cordY + 1 < height && cordX - 1 >= 0)
                                {
                                    aux = src + (cordY + 1) * bitmapDataSrc.Stride + (cordX - 1) * pixelSize;
                                    b5 = (*aux++);
                                    g5 = (*aux++);
                                    r5 = (*aux++);
                                }

                                if (cordY + 1 < height)
                                {
                                    aux = src + (cordY + 1) * bitmapDataSrc.Stride + cordX * pixelSize;
                                    b6 = (*aux++);
                                    g6 = (*aux++);
                                    r6 = (*aux++);
                                }

                                if (cordY + 1 < height && cordX + 1 < width)
                                {
                                    aux = src + (cordY + 1) * bitmapDataSrc.Stride + (cordX + 1) * pixelSize;
                                    b7 = (*aux++);
                                    g7 = (*aux++);
                                    r7 = (*aux++);
                                }

                                i = 0;
                                flag = 0;
                                while (i < 8 && flag != 1) // proximo p
                                {
                                    proximaDirecao = (direcao + 1 + i) % 8; // da onde eu vim

                                    if (proximaDirecao == 0 && cordX + 1 < width && r0 < 127 && g0 < 127 && b0 < 127)
                                    {
                                        cordX++;
                                        flag = 1;
                                    }
                                    else
                                    if (proximaDirecao == 1 && cordX + 1 < width && cordY - 1 >= 0 && r1 < 127 && g1 < 127 && b1 < 127)
                                    {
                                        cordY--;
                                        cordX++;
                                        flag = 1;
                                    }
                                    else
                                    if (proximaDirecao == 2 && cordY - 1 >= 0 && r2 < 127 && g2 < 127 && b2 < 127)
                                    {
                                        cordY--;
                                        flag = 1;
                                    }
                                    else
                                    if (proximaDirecao == 3 && cordY - 1 >= 0 && r3 < 127 && g3 < 127 && b3 < 127)
                                    {
                                        cordY--;
                                        cordX--;
                                        flag = 1;
                                    }
                                    else
                                    if (proximaDirecao == 4 && cordX - 1 >= 0 && r4 < 127 && g4 < 127 && b4 < 127)
                                    {
                                        cordX--;
                                        flag = 1;
                                    }
                                    else
                                    if (proximaDirecao == 5 && cordY + 1 < height && cordX - 1 >= 0 && r5 < 127 && g5 < 127 && b5 < 127)
                                    {

                                        cordY++;
                                        cordX--;
                                        flag = 1;
                                    }
                                    else
                                    if (proximaDirecao == 6 && cordY + 1 < height && r6 < 127 && g6 < 127 && b6 < 127)
                                    {
                                        cordY++;
                                        flag = 1;
                                    }
                                    else
                                    if (proximaDirecao == 7 && cordX + 1 < width && cordY + 1 < height && r7 < 127 && g7 < 127 && b7 < 127)
                                    {

                                        cordY++;
                                        cordX++;
                                        flag = 1;

                                    }

                                    if (flag == 1)
                                    {
                                        direcao = (proximaDirecao + 4) % 8;
                                        coordenadas[TL++] = cordY; // linha
                                        coordenadas[TL++] = cordX; // coluna
                                    }
                                    i++;
                                }



                            } while (cordX != inicioX || cordY != inicioY);
                        }

                    }
                }

                i = 0;
                while (i < TL)
                {
                    cordY = coordenadas[i++];
                    cordX = coordenadas[i++];

                    if (cordX + 1 < width)
                    {
                        aux = src + cordY * bitmapDataSrc.Stride + (cordX + 1) * pixelSize;
                        b0 = (*aux++);
                        g0 = (*aux++);
                        r0 = (*aux++);
                    }

                    if (cordX + 1 < width && cordY - 1 >= 0)
                    {
                        aux = src + (cordY - 1) * bitmapDataSrc.Stride + (cordX + 1) * pixelSize;
                        b1 = (*aux++);
                        g1 = (*aux++);
                        r1 = (*aux++);
                    }

                    if (cordY - 1 >= 0)
                    {
                        aux = src + (cordY - 1) * bitmapDataSrc.Stride + cordX * pixelSize;
                        b2 = (*aux++);
                        g2 = (*aux++);
                        r2 = (*aux++);
                    }

                    if (cordY - 1 >= 0 && cordX - 1 >= 0)
                    {
                        aux = src + (cordY - 1) * bitmapDataSrc.Stride + (cordX - 1) * pixelSize;
                        b3 = (*aux++);
                        g3 = (*aux++);
                        r3 = (*aux++);
                    }

                    if (cordX - 1 >= 0)
                    {
                        aux = src + cordY * bitmapDataSrc.Stride + (cordX - 1) * pixelSize;
                        b4 = (*aux++);
                        g4 = (*aux++);
                        r4 = (*aux++);
                    }

                    if (cordY + 1 < height && cordX - 1 >= 0)
                    {
                        aux = src + (cordY + 1) * bitmapDataSrc.Stride + (cordX - 1) * pixelSize;
                        b5 = (*aux++);
                        g5 = (*aux++);
                        r5 = (*aux++);
                    }

                    if (cordY + 1 < height)
                    {
                        aux = src + (cordY + 1) * bitmapDataSrc.Stride + cordX * pixelSize;
                        b6 = (*aux++);
                        g6 = (*aux++);
                        r6 = (*aux++);
                    }

                    if (cordY + 1 < height && cordX + 1 < width)
                    {
                        aux = src + (cordY + 1) * bitmapDataSrc.Stride + (cordX + 1) * pixelSize;
                        b7 = (*aux++);
                        g7 = (*aux++);
                        r7 = (*aux++);
                    }

                    if (r0 >= 127 && g0 >= 127 && b0 >= 127)
                    {
                        dstAux = dst + cordY * bitmapDataDst.Stride + (cordX + 1) * pixelSize;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                    }
                    if (r1 >= 127 && g1 >= 127 && b1 >= 127)
                    {
                        dstAux = dst + (cordY - 1) * bitmapDataDst.Stride + (cordX + 1) * pixelSize;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                    }
                    if (r2 >= 127 && g2 >= 127 && b2 >= 127)
                    {
                        dstAux = dst + (cordY - 1) * bitmapDataDst.Stride + cordX * pixelSize;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                    }
                    if (r3 >= 127 && g3 >= 127 && b3 >= 127)
                    {
                        dstAux = dst + (cordY - 1) * bitmapDataDst.Stride + (cordX - 1) * pixelSize;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                    }
                    if (r4 >= 127 && g4 >= 127 && b4 >= 127)
                    {
                        dstAux = dst + cordY * bitmapDataDst.Stride + (cordX - 1) * pixelSize;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                    }
                    if (r5 >= 127 && g5 >= 127 && b5 >= 127)
                    {
                        dstAux = dst + (cordY + 1) * bitmapDataDst.Stride + (cordX - 1) * pixelSize;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                    }
                    if (r6 >= 127 && g6 >= 127 && b6 >= 127)
                    {
                        dstAux = dst + (cordY + 1) * bitmapDataDst.Stride + cordX * pixelSize;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                    }
                    if (r7 >= 127 && g7 >= 127 && b7 >= 127)
                    {
                        dstAux = dst + (cordY + 1) * bitmapDataDst.Stride + (cordX + 1) * pixelSize;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                        *(dstAux++) = 0;
                    }

                }

            }

            //unlock imagem origem 
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }
    }

}

