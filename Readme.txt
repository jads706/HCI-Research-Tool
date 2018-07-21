*This is a Beta Version. Everything is subject to change.
Student Programmers: JaVon Sanchez, Jose Diaz
Professors: Ahmed Abdelzaher, Jon Suk Lee
Intro:
This is a tool that does a youtube search and gets the data of the video, the publisher related to the video, and comments along with replies. It then generates a txt file with that data and prints it out in json format. 
The purpose of this tool is so that it can get the text data of a youtube result and display it with ease. This can be very useful for text analysis. (Notepad ++ is highly recommended)

Application/executable is located in HCI Research Tool/bin/Debug Folder. It is named "HCI Research Tool".
To use Window App:

1. Type a word in the search bar
2. Type how many results
3. Click the order you want in Search Criteria
4. Click Generate
5. Name and save the file where you want to save it
6. Wait until the load bar is done loading(Paitence is a virtue)

Notes:
*On Average 50 results will take around 30 minutes.
*There is an issue where if there is a spammer on a video, then it could potentially fail because the comements are rapidly changing and cause the api server failure. This error can also be caused by a any comment that is posted
while the program is searching in that particular thread. 
*At the moment the stopwatch updates with the loading bar; however, the stopwatch still shows an accurate elapsed time.
