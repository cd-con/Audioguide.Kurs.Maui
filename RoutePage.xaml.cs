using Plugin.Maui.Audio;

namespace Audioguide.Kurs.Maui;

public partial class RoutePage : ContentPage
{

    private IAudioManager _audioManager;
    private IAudioPlayer _player;
    private Stream _aStream;
    public RoutePage(Tour tour)
    {
        InitializeComponent();
        BindingContext = tour;
        _audioManager = AudioManager.Current;

    }

    /// <summary>
    /// ������ ������ �������������. ������� �� ������ ���������, ���� ������ �� ������;
    /// ������ �� �����, ���� ������ ������; ���������� ���������������, ���� ������ �� �����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnPlayAudioClicked(object sender, EventArgs e)
    {
        if (_aStream is not null)
            _player ??= _audioManager.CreatePlayer(_aStream); // ���������� if (_player != null)
        else
            await DisplayAlert("����� ����...", "���� ��� ��������������� �� ��� ������.", "�� ����� ���");


        if ((BindingContext is Tour tour) && _player is not null)
        {
            if (_player.IsPlaying)
                _player.Pause();
            else
                _player.Play();
        }
    }

    /// <summary>
    /// ����������� ����������� ��� �������� ��������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        try
        {
            if (BindingContext is Tour tour)
                _aStream = await FileSystem.OpenAppPackageFileAsync(tour.AudioFile);
        }
        catch (Exception err)
        {
            await DisplayAlert("������!", err.Message, "�� ����� ���");
        }
    }


    /// <summary>
    /// �������� ������ 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
        _player?.Stop();
        _player?.Dispose();
    }
}