import glob
import subprocess

files = glob.glob('./publish/*/*')
for f in files:
    rid = f.replace('\\', '/').split('/')[-2]
    subprocess.run(f"7z a ./publish/{rid}.zip {f}")
