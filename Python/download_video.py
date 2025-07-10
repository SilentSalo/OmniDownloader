import sys
import yt_dlp


def download_video(url):
    ydl_opts = {
        'format': 'best',
        'outtmpl': '-',
        'quiet': True
    }

    try:
        with yt_dlp.YoutubeDL(ydl_opts) as ydl:
            result = ydl.extract_info(url, download=False)

            with ydl.urlopen(result['url']) as response:
                video_bytes = response.read()
                sys.stdout.buffer.write(video_bytes)
                sys.stdout.flush()
                print("Process (download_video.exe) exited successfully.",
                      file=open('python/last_video_result.txt', 'w'))
                return
    except Exception as e:
        print(f"Process (download_video.exe) exited with error: {str(e)}",
              file=open('python/last_video_result.txt', 'w'))
        return None


if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: download_video.py <url>", file=open('python/last_video_result.txt', 'w'))
        sys.exit(1)

    video_url = sys.argv[1]
    download_video(video_url)
