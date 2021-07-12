using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MediaPlayer;

namespace MyMediaPlayer
{
    public class MediaDal
    {
        //Kullandığım veritabanı SaqlConnection nesnesi üzeirnden bağladım.
        SqlConnection _sql = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBMedia;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");

        public List<Media> MediaListele()
        {
            BaglantiKontrol();
            SqlCommand _command = new SqlCommand("select * from  dbo.MyMedias", _sql);
            SqlDataReader reader = _command.ExecuteReader(); //veritabanından değerimizi okuduk.
            List<Media> videos = new List<Media>();

            while (reader.Read())
            {
                Media video = new Media()
                {
                    //nesnelerimi okudum ve kendi türlerine çevirdim.
                    Id = (int)reader[0],
                    mediaName = Convert.ToString(reader[1]),
                    mediaTarih = Convert.ToString(reader[2]),
                };
                //listeme nesnemi ekledim.
                videos.Add(video);
            }
            reader.Close();
            _sql.Close();
            return videos;
        }


        /// <summary>
        /// Bağlantıyı kontrol edip SqlCommand ile eklemek istediğim nesneyi belirttim ve value değeleri atadım.
        /// </summary>
        /// <param name="video"></param>
        public void Ekle(Media media)
        {
            BaglantiKontrol();
            SqlCommand _command2 = new SqlCommand("insert into MyMedias values(@name,@tarih)", _sql);
            _command2.Parameters.AddWithValue("@name", media.mediaName);  //nesnemin value değerini diğer nesnemin neresinden alacağını belirttim.
            _command2.Parameters.AddWithValue("@tarih", media.mediaTarih);
            _command2.ExecuteNonQuery();
            _sql.Close();

        }
        /// <summary>
        /// Update komutuyla güncelledim.
        /// </summary>
        /// <param name="media"></param>
        public void Guncelle(Media media)
        {

            BaglantiKontrol();
            SqlCommand _command3 = new SqlCommand("Update MyMedias set mediaName=@name, mediaTarih=@tarih where Id=@id", _sql);
            _command3.Parameters.AddWithValue("@id", media.Id);
            _command3.Parameters.AddWithValue("@name", media.mediaName);  //nesnemin value değerini diğer nesnemin neresinden alacağını belirttim.
            _command3.Parameters.AddWithValue("@tarih", media.mediaTarih);
            _command3.ExecuteNonQuery();
            _sql.Close();

        }

        public void Sil(int id)
        {

            BaglantiKontrol();
            SqlCommand _command3 = new SqlCommand("Delete from MyMedias where Id=@id", _sql);
            _command3.Parameters.AddWithValue("@id", id);
            //nesnemin value değerini diğer nesnemin neresinden alacağını belirttim.

            _command3.ExecuteNonQuery();
            _sql.Close();

        }

        /// <summary>
        /// eğer bağlantı kapalıysa açsın yoksa hiçç bakmasın.
        /// </summary>
        private void BaglantiKontrol()
        {

            if (_sql.State == ConnectionState.Closed)
            {
                _sql.Open();
            }
        }

    }
}
