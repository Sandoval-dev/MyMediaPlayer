using MediaPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer;
        MediaDal mediaDal = new MediaDal();


        public MainWindow()
        {

            InitializeComponent();
            //timer nesnemi tanımladım.
            timer = new DispatcherTimer();

            //timer nesnemin 100 milisaniyede bir çalıştığını belirttim.
            timer.Interval = TimeSpan.FromMilliseconds(100);

            //timer nesneme Tick eventini yükledim.
            timer.Tick += Timer_Tick;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MediaListele();
        }
        /// <summary>
        /// Mediaları listelemek için yazdığım metot
        /// </summary>
        private void MediaListele()
        {
            dtgMedia.ItemsSource = mediaDal.MediaListele();
        }

        /// <summary>
        /// slider_seek nesnemin değerini media nesnemein toplam saniyesine eşitledim.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            slider_seek.Value = media_element1.Position.TotalSeconds;
        }

        /// <summary>
        /// Play butonu için yazdığım kod satırı
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            media_element1.Play();

        }

        /// <summary>
        /// Pause butonu için yazdığım kod satırı
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            media_element1.Pause();
        }

        /// <summary>
        /// Stop butonu için yazdığım kod satırı
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            media_element1.Stop();
        }

        /// <summary>
        /// media elementimin ses değerini slider_volume un değerine eşitledim.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slider_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media_element1.Volume = (double)slider_volume.Value;
        }

        /// <summary>
        /// media elementimin uzunluğunu slider_seek nesnemin uzunluğuna eşitledim.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slider_seek_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media_element1.Position = TimeSpan.FromSeconds(slider_seek.Value);
        }

        /// <summary>
        /// Sürükle bırak metodunda  string tanımladığım filename nesneme bıraktığım nesneyi string türüne çevirip eşitledim
        /// ve media nesnemin kaynağının bu nesne olduğunu belirttim.
        /// medyanın yükleme ve yüklenmemiş halini alıp el ile denetlenmesini söyledim.
        /// bıraktığımız nesnenin ses değerini eşitledim
        /// son olarak dosyamızın yolunu ve ismini gösterecek kod satırını yazdım.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Drop(object sender, DragEventArgs e)
        {
            string filename = (string)((DataObject)e.Data).GetFileDropList()[0];
            media_element1.Source = new Uri(filename);

            media_element1.LoadedBehavior = MediaState.Manual;
            media_element1.UnloadedBehavior = MediaState.Manual;
            media_element1.Volume = (double)slider_volume.Value;



            mylabel.Content = filename;


        }


        /// <summary>
        /// timespan sınıfından nesne oluşturup media elementimin doğal süresini aldım.
        /// sonra slider_seek nesnemin maksimum değerini aldığım toplam süreye eşitledim.
        /// sayacımı başlattım
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void media_element1_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = media_element1.NaturalDuration.TimeSpan;
            slider_seek.Maximum = ts.TotalSeconds;
            timer.Start();
        }

        /// <summary>
        /// buttondan ayrılınca gerçekleşmesini istediğim olayı yazdım.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_MouseLeave(object sender, MouseEventArgs e)
        {
            button1.Background = Brushes.LawnGreen;
        }





        private void MediaPlayer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
        /// <summary>
        /// buttondan ayrılınca gerçekleşmesini istediğim olayı yazdım.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_MouseLeave(object sender, MouseEventArgs e)
        {
            button3.Background = Brushes.Red;
        }

        /// <summary>
        /// Ekle butonuna tıklayınca nesne oluşturup ekliyoruz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEkle_Click(object sender, RoutedEventArgs e)
        {
            mediaDal.Ekle(new Media
            {
                mediaName = videoNametxt.Text,
                mediaTarih = Convert.ToString(Tarihtxt.Text),
            });
            MediaListele();
        }

        /// <summary>
        /// Media sınıfından nesne oluşturdum sonra bu nesneyi datagrid seçili itemine tür
        /// dönüşümü yapıp eşitledim sonra seçilince grid e gelmesini sağladım.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtgMedia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Media media = new Media();
            media = (Media)dtgMedia.SelectedItem;
            grid1.DataContext = media;
        }

        /// <summary>
        /// textboxları temizlemek için yazdığım kod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTemizle_Click(object sender, RoutedEventArgs e)
        {
            Idtxt.Text = string.Empty;
            videoNametxt.Text = string.Empty;
            Tarihtxt.Text = string.Empty;

        }
        /// <summary>
        /// Silme metotumuz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(Idtxt.Text);
            mediaDal.Sil(id);
            MediaListele();

        }

        /// <summary>
        /// Güncelleme metotumuz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            mediaDal.Guncelle(new Media
            {
                Id = Convert.ToInt32(Idtxt.Text),
                mediaName = videoNametxt.Text,
                mediaTarih = Tarihtxt.Text,
            });
            MediaListele();
        }

        /// <summary>
        /// item aradığımız zaman onu datagride aktaracak kodu yazdık.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAra_SelectionChanged(object sender, RoutedEventArgs e)
        {
            dtgMedia.ItemsSource = mediaDal.MediaListele().Where(x => x.mediaName.ToLower().Contains(txtAra.Text.ToLower())).ToList();
        }
    }
}
