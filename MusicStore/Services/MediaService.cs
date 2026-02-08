using MusicStore.Dtos;

namespace MusicStore.Services;

using SkiaSharp;
using System.Text;

public class MediaService : IMediaService
{
    private readonly string _fontPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Roboto-Bold.ttf");

    public byte[] GenerateCover(MediaDto dto)
    {
        const int size = 512;
        using var bitmap = new SKBitmap(size, size);
        using var canvas = new SKCanvas(bitmap);
        
        var r = (byte)((dto.Seed * 123) % 256);
        var g = (byte)((dto.Seed * 456) % 256);
        var b = (byte)((dto.Seed * 789) % 256);
        canvas.Clear(new SKColor(r, g, b));

        var rand = new Random((int)(dto.Seed % int.MaxValue));
        for (int i = 0; i < 10; i++)
        {
            using var paint = new SKPaint
            {
                Color = new SKColor((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256), 80),
                IsAntialias = true
            };
            canvas.DrawCircle(rand.Next(size), rand.Next(size), rand.Next(40, 150), paint);
        }

        using var typeface = SKTypeface.FromFile(_fontPath);
        using var textPaint = new SKPaint
        {
            Typeface = typeface,
            Color = SKColors.White,
            TextAlign = SKTextAlign.Center,
            IsAntialias = true,
            TextSize = 48
        };
        
        canvas.DrawText(dto.Title, size / 2, size / 2, textPaint);
        textPaint.TextSize = 24;
        textPaint.Color = SKColors.LightGray;
        canvas.DrawText(dto.Artist, size / 2, (size / 2) + 40, textPaint);

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    public byte[] GenerateAudio(long seed)
{
    int sampleRate = 44100;
    int durationSeconds = 5;
    int numSamples = sampleRate * durationSeconds;
    short[] waveData = new short[numSamples];
    
    var rand = new Random((int)(seed % int.MaxValue));
    
    double bpm = 120; 
    double samplesPerBeat = (sampleRate * 60) / bpm;
    
    double[][] progressions = {
        new double[] { 261.63, 329.63, 392.00 },
        new double[] { 220.00, 261.63, 329.63 }, 
        new double[] { 174.61, 220.00, 261.63 }, 
        new double[] { 196.00, 246.94, 293.66 }  
    };

    for (int i = 0; i < numSamples; i++)
    {
        double time = (double)i / sampleRate;
        int beat = (int)(i / samplesPerBeat);
        int measure = (beat / 4) % progressions.Length;
        double beatPosition = (i % samplesPerBeat) / samplesPerBeat;


        double bassFreq = progressions[measure][0] / 2;
        double bassWave = (2.0 / Math.PI) * Math.Asin(Math.Sin(2 * Math.PI * bassFreq * time));
        double bassEnv = 0.6 + (0.4 * beatPosition); 
        
        double melodyFreq = progressions[measure][(seed + beat) % 3] * 2;
        double melodyWave = Math.Sin(2 * Math.PI * melodyFreq * time);
        double melodyEnv = Math.Max(0, 1.0 - (beatPosition * 2.0));
        
        double noiseWave = 0;
        if (beat % 2 == 1 && beatPosition < 0.1) {
            noiseWave = (rand.NextDouble() * 2.0 - 1.0) * (1.0 - beatPosition * 10);
        }
        
        double mixed = (bassWave * 0.4 * bassEnv) + 
                       (melodyWave * 0.3 * melodyEnv) + 
                       (noiseWave * 0.2);

        waveData[i] = (short)(mixed * 12000);
    }

    return CreateWavWithHeader(waveData, sampleRate);
}
    private byte[] CreateWavWithHeader(short[] samples, int sampleRate)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        writer.Write(Encoding.ASCII.GetBytes("RIFF"));
        writer.Write(36 + samples.Length * 2);
        writer.Write(Encoding.ASCII.GetBytes("WAVE"));
        writer.Write(Encoding.ASCII.GetBytes("fmt "));
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)1);
        writer.Write(sampleRate);
        writer.Write(sampleRate * 2);
        writer.Write((short)2);
        writer.Write((short)16);
        writer.Write(Encoding.ASCII.GetBytes("data"));
        writer.Write(samples.Length * 2);

        foreach (var sample in samples) writer.Write(sample);

        return stream.ToArray();
    }
}