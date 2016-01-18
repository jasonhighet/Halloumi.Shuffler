using System.Collections.Generic;
using System.Linq;

namespace Halloumi.Shuffler.Engine.Models
{
    public class GenreCode
    {
        public GenreCode(int number, string name)
        {
            Number = number;
            Name = name;
        }

        public static bool IsGenreCode(string code)
        {
            code = code.Trim();
            return code.StartsWith("(");
        }


        public static string GetGenre(string code)
        {
            var genreCode = GetAllGenres().Where(g => g.Code == code).FirstOrDefault();
            if (genreCode == null) return "";
            return genreCode.Name;
        }

        public int Number { get; set; }
        public string Name { get; set; }
        public string Code
        {
            get { return string.Format("({0})", Number); }
        }

        private static List<GenreCode> GetAllGenres()
        {
            var genreCodes = new List<GenreCode>();

            genreCodes.Add(new GenreCode(0, "Blues "));
            genreCodes.Add(new GenreCode(1, "Classic Rock"));
            genreCodes.Add(new GenreCode(2, "Country"));
            genreCodes.Add(new GenreCode(3, "Dance"));
            genreCodes.Add(new GenreCode(4, "Disco"));
            genreCodes.Add(new GenreCode(5, "Funk"));
            genreCodes.Add(new GenreCode(6, "Grunge"));
            genreCodes.Add(new GenreCode(7, "Hip-Hop"));
            genreCodes.Add(new GenreCode(8, "Jazz"));
            genreCodes.Add(new GenreCode(9, "Metal"));
            genreCodes.Add(new GenreCode(10, "New Age"));
            genreCodes.Add(new GenreCode(11, "Oldies"));
            genreCodes.Add(new GenreCode(12, "Other"));
            genreCodes.Add(new GenreCode(13, "Pop"));
            genreCodes.Add(new GenreCode(14, "R&B"));
            genreCodes.Add(new GenreCode(15, "Rap"));
            genreCodes.Add(new GenreCode(16, "Reggae"));
            genreCodes.Add(new GenreCode(17, "Rock"));
            genreCodes.Add(new GenreCode(18, "Techno"));
            genreCodes.Add(new GenreCode(19, "Industrial"));
            genreCodes.Add(new GenreCode(20, "Alternative"));
            genreCodes.Add(new GenreCode(21, "Ska"));
            genreCodes.Add(new GenreCode(22, "Death Metal"));
            genreCodes.Add(new GenreCode(23, "Pranks"));
            genreCodes.Add(new GenreCode(24, "Soundtrack"));
            genreCodes.Add(new GenreCode(25, "Euro-Techno"));
            genreCodes.Add(new GenreCode(26, "Ambient"));
            genreCodes.Add(new GenreCode(27, "Trip-Hop"));
            genreCodes.Add(new GenreCode(28, "Vocal"));
            genreCodes.Add(new GenreCode(29, "Jazz+Funk"));
            genreCodes.Add(new GenreCode(30, "Fusion"));
            genreCodes.Add(new GenreCode(31, "Trance"));
            genreCodes.Add(new GenreCode(32, "Classical"));
            genreCodes.Add(new GenreCode(33, "Instrumental"));
            genreCodes.Add(new GenreCode(34, "Acid"));
            genreCodes.Add(new GenreCode(35, "House"));
            genreCodes.Add(new GenreCode(36, "Game"));
            genreCodes.Add(new GenreCode(37, "Sound Clip"));
            genreCodes.Add(new GenreCode(38, "Gospel"));
            genreCodes.Add(new GenreCode(39, "Noise"));
            genreCodes.Add(new GenreCode(40, "AlternRock"));
            genreCodes.Add(new GenreCode(41, "Bass"));
            genreCodes.Add(new GenreCode(42, "Soul"));
            genreCodes.Add(new GenreCode(43, "Punk"));
            genreCodes.Add(new GenreCode(44, "Space"));
            genreCodes.Add(new GenreCode(45, "Meditative"));
            genreCodes.Add(new GenreCode(46, "Instrumental Pop"));
            genreCodes.Add(new GenreCode(47, "Instrumental Rock"));
            genreCodes.Add(new GenreCode(48, "Ethnic"));
            genreCodes.Add(new GenreCode(49, "Gothic"));
            genreCodes.Add(new GenreCode(50, "Darkwave"));
            genreCodes.Add(new GenreCode(51, "Techno-Industrial"));
            genreCodes.Add(new GenreCode(52, "Electronic"));
            genreCodes.Add(new GenreCode(53, "Pop-Folk"));
            genreCodes.Add(new GenreCode(54, "Eurodance"));
            genreCodes.Add(new GenreCode(55, "Dream"));
            genreCodes.Add(new GenreCode(56, "Southern Rock"));
            genreCodes.Add(new GenreCode(57, "Comedy"));
            genreCodes.Add(new GenreCode(58, "Cult"));
            genreCodes.Add(new GenreCode(59, "Gangsta"));
            genreCodes.Add(new GenreCode(60, "Top 40"));
            genreCodes.Add(new GenreCode(61, "Christian Rap"));
            genreCodes.Add(new GenreCode(62, "Pop/Funk"));
            genreCodes.Add(new GenreCode(63, "Jungle"));
            genreCodes.Add(new GenreCode(64, "Native American"));
            genreCodes.Add(new GenreCode(65, "Cabaret"));
            genreCodes.Add(new GenreCode(66, "New Wave"));
            genreCodes.Add(new GenreCode(67, "Psychadelic"));
            genreCodes.Add(new GenreCode(68, "Rave"));
            genreCodes.Add(new GenreCode(69, "Showtunes"));
            genreCodes.Add(new GenreCode(70, "Trailer"));
            genreCodes.Add(new GenreCode(71, "Lo-Fi"));
            genreCodes.Add(new GenreCode(72, "Tribal"));
            genreCodes.Add(new GenreCode(73, "Acid Punk"));
            genreCodes.Add(new GenreCode(74, "Acid Jazz"));
            genreCodes.Add(new GenreCode(75, "Polka"));
            genreCodes.Add(new GenreCode(76, "Retro"));
            genreCodes.Add(new GenreCode(77, "Musical"));
            genreCodes.Add(new GenreCode(78, "Rock & Roll"));
            genreCodes.Add(new GenreCode(79, "Hard Rock"));
            genreCodes.Add(new GenreCode(80, "Folk"));
            genreCodes.Add(new GenreCode(81, "Folk-Rock"));
            genreCodes.Add(new GenreCode(82, "National Folk"));
            genreCodes.Add(new GenreCode(83, "Swing"));
            genreCodes.Add(new GenreCode(84, "Fast Fusion"));
            genreCodes.Add(new GenreCode(85, "Bebob"));
            genreCodes.Add(new GenreCode(86, "Latin"));
            genreCodes.Add(new GenreCode(87, "Revival"));
            genreCodes.Add(new GenreCode(88, "Celtic"));
            genreCodes.Add(new GenreCode(89, "Bluegrass"));
            genreCodes.Add(new GenreCode(90, "Avantgarde"));
            genreCodes.Add(new GenreCode(91, "Gothic Rock"));
            genreCodes.Add(new GenreCode(92, "Progressive Rock"));
            genreCodes.Add(new GenreCode(93, "Psychedelic Rock"));
            genreCodes.Add(new GenreCode(94, "Symphonic Rock"));
            genreCodes.Add(new GenreCode(95, "Slow Rock"));
            genreCodes.Add(new GenreCode(96, "Big Band"));
            genreCodes.Add(new GenreCode(97, "Chorus"));
            genreCodes.Add(new GenreCode(98, "Easy Listening"));
            genreCodes.Add(new GenreCode(99, "Acoustic"));
            genreCodes.Add(new GenreCode(100, "Humour"));
            genreCodes.Add(new GenreCode(101, "Speech"));
            genreCodes.Add(new GenreCode(102, "Chanson"));
            genreCodes.Add(new GenreCode(103, "Opera"));
            genreCodes.Add(new GenreCode(104, "Chamber Music"));
            genreCodes.Add(new GenreCode(105, "Sonata"));
            genreCodes.Add(new GenreCode(106, "Symphony"));
            genreCodes.Add(new GenreCode(107, "Booty Bass"));
            genreCodes.Add(new GenreCode(108, "Primus"));
            genreCodes.Add(new GenreCode(109, "Porn Groove"));
            genreCodes.Add(new GenreCode(110, "Satire"));
            genreCodes.Add(new GenreCode(111, "Slow Jam"));
            genreCodes.Add(new GenreCode(112, "Club"));
            genreCodes.Add(new GenreCode(113, "Tango"));
            genreCodes.Add(new GenreCode(114, "Samba"));
            genreCodes.Add(new GenreCode(115, "Folklore"));
            genreCodes.Add(new GenreCode(116, "Ballad"));
            genreCodes.Add(new GenreCode(117, "Power Ballad"));
            genreCodes.Add(new GenreCode(118, "Rhythmic Soul"));
            genreCodes.Add(new GenreCode(119, "Freestyle"));
            genreCodes.Add(new GenreCode(120, "Duet"));
            genreCodes.Add(new GenreCode(121, "Punk Rock"));
            genreCodes.Add(new GenreCode(122, "Drum Solo"));
            genreCodes.Add(new GenreCode(123, "A capella"));
            genreCodes.Add(new GenreCode(124, "Euro-House"));
            genreCodes.Add(new GenreCode(125, "Dance Hall"));
            return genreCodes;
        }

    }
}
