namespace Audioguide.Kurs.Maui
{
    /// <summary>
    /// Модель данных аудиотура
    /// </summary>
    public class Tour
    {
        public string Name { get; set; }
        public string Duration { get; set; }
        public string PreviewImage { get => string.Empty; set => PreviewImageSource = ImageSource.FromFile(value); }

        public ImageSource? PreviewImageSource { get; set; }
        public string MapImage { get; set; }
        public string AudioFile { get; set; }

    }
}