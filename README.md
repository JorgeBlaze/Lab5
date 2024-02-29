A.L.H.
# Lab5

Using AllTalk API with C#

Issue list
1. Cloning alltalk repo from GitHub:I used git installed locally to clone the repo, but it wouldn't take an arguement for a destination folder so it just cloned to a temp folder. I bypassed this and placed alltalk in the lab5 files so it could be installed from there--just need to change the path
2. Opening alltalk service in a window: I thought this would be easy, but it took a while to get the service to open in a new cmd window instead of taking over the program window--I used the ProcessStartInfo class with createnowindow = false and that worked
3. Had to add some expection handeling to the service start check--also added a task.delay to keep trying until the service returned a ready message
4. Missing features: the voice list; they can be found in the tts folder under voices. Return message; the audio file will play automatically upon completetion with the way the api call is currently set up, but it would be nice to have a done! message display in the console
5. Installing alltalk on another PC: after it was working on my desktop, I tried going through the install process on my laptop, but I kept on running out of space (256gb ssd) and having to reinstall. I don't know if it was because of that, but I couldn't get it working. 
