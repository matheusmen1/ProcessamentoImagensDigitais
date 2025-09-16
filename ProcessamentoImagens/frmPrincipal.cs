using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace ProcessamentoImagens
{
    public struct Coordenada
    {

        private int y;
        private int x;

        public Coordenada(int y, int x)
        {
            this.y = y;
            this.x = x;
        }
        public int getX()
        {
            return this.x;
        }
        public int getY()
        {
            return this.y;
        }
    }
    public partial class frmPrincipal : Form
    {

        private Image image;
        private Bitmap imageBitmap;

        List<List<Coordenada>> contornos = new List<List<Coordenada>>();

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnAbrirImagem_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Arquivos de Imagem (*.jpg;*.gif;*.bmp;*.png)|*.jpg;*.gif;*.bmp;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog.FileName);
                pictBoxImg1.Image = image;
                pictBoxImg1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            pictBoxImg1.Image = null;
            pictBoxImg2.Image = null;
        }

        private void btnLuminanciaSemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.convert_to_gray(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnLuminanciaComDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.convert_to_grayDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnNegativoSemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.negativo(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnNegativoComDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.negativoDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnEspelharVerticalSemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.espelharVertical(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
            pictBoxImg2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnEspelharHorizontalSemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.espelharHorizontal(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
            pictBoxImg2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnAfinamentoDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.afinamentoDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
            pictBoxImg2.SizeMode = PictureBoxSizeMode.Zoom;
            imgDest.Save("D:\\imagem_com_afinamento.png", ImageFormat.Png);
        }

        private void btncontourFollowingDMA_Click(object sender, EventArgs e)
        {

            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            this.contornos.Clear();
            Filtros.contourFollowingDMA(imageBitmap, imgDest, this.contornos);
            pictBoxImg2.Image = imgDest;
            pictBoxImg2.SizeMode = PictureBoxSizeMode.Zoom;
            imgDest.Save("D:\\imagem_com_extracao_contorno.png", ImageFormat.Png);
        }

        private void btnRetanguloMinimo_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.retanguloMinimoDMA(imageBitmap, imgDest, this.contornos);
            pictBoxImg2.Image = imgDest;
            pictBoxImg2.SizeMode = PictureBoxSizeMode.Zoom;
            imgDest.Save("D:\\imagem_com_retangulo_minimo.png", ImageFormat.Png);
        }
    }
}
