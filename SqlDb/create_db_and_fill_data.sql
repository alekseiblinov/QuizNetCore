use [quiz]
GO

--set foreign_key_checks = 0;
--DROP SCHEMA IF EXISTS L693;
--CREATE SCHEMA L693;
--USE L693;

--CREATE TABLE "quiz" (
--    "question_id" uniqueidentifier NOT NULL,
--    "category" nvarchar(50) NOT NULL,
--    "quiz_contact" nvarchar(250) NOT NULL,
--    "option_a" nvarchar(250),
--    "option_b" nvarchar(250),
--    "option_c" nvarchar(250),
--    "option_d" nvarchar(250),
--    "option_e" nvarchar(250),
--    "option_f" nvarchar(250),
--    "option_g" nvarchar(250),
--    "answer" nvarchar(250)
--) 

--CREATE TABLE "users" (
--    "user_id" uniqueidentifier NOT NULL,
--    "username" nvarchar(100) NOT NULL,
--    "email" nvarchar(100) NOT NULL,
--    "password" nvarchar(100) NOT NULL
--);

--CREATE TABLE "user_answers" (
--    "answer_id" uniqueidentifier NOT NULL,
--    "user_id" INT NOT NULL,
--    "question_id" INT NOT NULL,
--    "user_answer" nvarchar(250) NOT NULL,
--    "date_time" DATETIME NOT NULL 
--);

-----
INSERT INTO "quiz" 
("category", "quiz_contact", "option_a", "option_b", "option_c", "option_d", "answer") 
VALUES 
('HTML', 'What does HTML stand for?', 'a) HyperText Markup Language', 'b) HyperText Markdown Language', 'c) Hyperlink and Text Markup Language', 'd) Hyperlink and Text Markdown Language', 'Answer: a')
('HTML', 'Which of the following is the correct syntax to create a hyperlink in HTML?', 'a) <a url="http://example.com">Link</a>', 'b) <a href="http://example.com">Link</a>', 'c) <a link="http://example.com">Link</a>', 'd) <link href="http://example.com">Link</link>', 'Answer: b')
('HTML', 'Which tag is used to create a paragraph in HTML?', 'a) <p>', 'b) <para>', 'c) <paragraph>', 'd) <pg>', 'Answer: a')
('HTML', 'What is the correct way to add an image in HTML?', 'a) <img src="image.jpg" alt="Image description">', 'b) <image src="image.jpg" description="Image description">', 'c) <img href="image.jpg" text="Image description">', 'd) <image href="image.jpg" alt="Image description">', 'Answer: a')
('HTML', 'Which HTML tag is used to define an unordered list?', 'a) <ul>', 'b) <ol>', 'c) <li>', 'd) <list>', 'Answer: a')

('MySQL', 'Which SQL statement is used to retrieve data from a database?', 'a) SELECT', 'b) GET', 'c) FETCH', 'd) EXTRACT', 'Answer: a')
('MySQL', 'Which of the following is used to eliminate duplicate rows in a SQL query result?', 'a) DISTINCT', 'b) UNIQUE', 'c) REMOVE', 'd) DELETE', 'Answer: a')
('MySQL', 'What is the correct SQL statement to create a new table named Students?', 'a) CREATE Students', 'b) CREATE TABLE Students', 'c) NEW TABLE Students', 'd) ADD TABLE Students', 'Answer: b')
('MySQL', 'Which SQL keyword is used to sort the result-set?', 'a) SORT', 'b) ORDER', 'c) ARRANGE', 'd) ORGANIZE', 'Answer: b')
('MySQL', 'Which clause is used to filter the results returned by a SELECT query?', 'a) WHERE', 'b) FILTER', 'c) LIMIT', 'd) GROUP', 'Answer: a')

('Python', 'What is the correct way to create a function in Python?', 'a) def functionName:', 'b) function functionName():', 'c) def functionName():', 'd) create function functionName()', 'Answer: c')
('Python', 'Which of the following is a mutable data type in Python?', 'a) String', 'b) Tuple', 'c) List', 'd) Integer', 'Answer: c')
('Python', 'What does the len() function do?', 'a) Returns the number of characters in a string', 'b) Returns the length of a list', 'c) Returns the number of elements in a list', 'd) All of the above', 'Answer: d')
('Python', 'What is the output of 3 + 2 * 2?', 'a) 7', 'b) 10', 'c) 9', 'd) 8', 'Answer: a')
('Python', 'Which of the following is used to start a loop in Python?', 'a) repeat', 'b) for', 'c) loop', 'd) iterate', 'Answer: b')

('CSS', 'What does CSS stand for?', 'a) Cascading Style Sheets', 'b) Computer Style Sheets', 'c) Creative Style Sheets', 'd) Colorful Style Sheets', 'Answer: a')
('CSS', 'Which of the following is the correct syntax to apply a background color in CSS?', 'a) background-color: blue', 'b) bg-color: blue', 'c) color-background: blue', 'd) color-bg: blue', 'Answer: a')
('CSS', 'How do you add an external CSS file to an HTML document?', 'a) <link rel="stylesheet" type="text/css" href="styles.css">', 'b) <style src="styles.css"></style>', 'c) <stylesheet link="styles.css"></stylesheet>', 'd) <link src="styles.css" type="text/css">', 'Answer: a')
('CSS', 'Which of the following selectors is used to target an element with a specific ID in CSS?', 'a) .myElement', 'b) #myElement', 'c) *myElement', 'd) &myElement', 'Answer: b')
('CSS', 'What is the correct way to make text bold using CSS?', 'a) font-weight: bold', 'b) font-style: bold', 'c) text-weight: bold', 'd) font-size: bold', 'Answer: a');


--INSERT INTO "users" ("username", "email", "password")
--VALUES
--('John', 'john@example.com', '123')
--('Jane', 'jane@example.com', '123');

INSERT INTO "user_answers" ("user_id", "question_id", "user_answer")
VALUES
(1, 1000, 'a')
(2, 1001, 'b')
(1, 1002, 'c')
(2, 1003, 'd');
