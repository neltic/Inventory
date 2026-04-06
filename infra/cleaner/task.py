import os
import time

PATH = "/temp_files"

SECONDS_TO_EXPIRE = 86400 

print("--- Temporary cleaning service started ---")

while True:
    try:
        now = time.time()
        files_deleted = 0
        
        for f in os.listdir(PATH):
            f_path = os.path.join(PATH, f)            
            if os.path.isfile(f_path):               
                if os.stat(f_path).st_mtime < now - SECONDS_TO_EXPIRE:
                    os.remove(f_path)
                    print(f"[{time.strftime('%Y-%m-%d %H:%M:%S')}] Deleted: {f}")
                    files_deleted += 1
        
        if files_deleted == 0:
            print(f"[{time.strftime('%Y-%m-%d %H:%M:%S')}] Review completed. Nothing to delete.")
            
    except Exception as e:
        print(f"Error in the cleaning cycle: {e}")

    time.sleep(3600)