USE [quiz]
GO

--delete from [dbo].[Question]
--GO

INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), 'DC0227A8-8E2A-48B5-8DC2-EF03C37BFE83', 'What does HTML stand for?', 'a) HyperText Markup Language', 'b) HyperText Markdown Language', 'c) Hyperlink and Text Markup Language', 'd) Hyperlink and Text Markdown Language', 'Answer: a')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), 'DC0227A8-8E2A-48B5-8DC2-EF03C37BFE83', 'Which of the following is the correct syntax to create a hyperlink in HTML?', 'a) <a url="http://example.com">Link</a>', 'b) <a href="http://example.com">Link</a>', 'c) <a link="http://example.com">Link</a>', 'd) <link href="http://example.com">Link</link>', 'Answer: b')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), 'DC0227A8-8E2A-48B5-8DC2-EF03C37BFE83', 'Which tag is used to create a paragraph in HTML?', 'a) <p>', 'b) <para>', 'c) <paragraph>', 'd) <pg>', 'Answer: a')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), 'DC0227A8-8E2A-48B5-8DC2-EF03C37BFE83', 'What is the correct way to add an image in HTML?', 'a) <img src="image.jpg" alt="Image description">', 'b) <image src="image.jpg" description="Image description">', 'c) <img href="image.jpg" text="Image description">', 'd) <image href="image.jpg" alt="Image description">', 'Answer: a')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), 'DC0227A8-8E2A-48B5-8DC2-EF03C37BFE83', 'Which HTML tag is used to define an unordered list?', 'a) <ul>', 'b) <ol>', 'c) <li>', 'd) <list>', 'Answer: a')

INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), '261FA5CE-5DA9-488C-97B1-57E89017621F', 'Which SQL statement is used to retrieve data from a database?', 'a) SELECT', 'b) GET', 'c) FETCH', 'd) EXTRACT', 'Answer: a')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), '261FA5CE-5DA9-488C-97B1-57E89017621F', 'Which of the following is used to eliminate duplicate rows in a SQL query result?', 'a) DISTINCT', 'b) UNIQUE', 'c) REMOVE', 'd) DELETE', 'Answer: a')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), '261FA5CE-5DA9-488C-97B1-57E89017621F', 'What is the correct SQL statement to create a new table named Students?', 'a) CREATE Students', 'b) CREATE TABLE Students', 'c) NEW TABLE Students', 'd) ADD TABLE Students', 'Answer: b')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), '261FA5CE-5DA9-488C-97B1-57E89017621F', 'Which SQL keyword is used to sort the result-set?', 'a) SORT', 'b) ORDER', 'c) ARRANGE', 'd) ORGANIZE', 'Answer: b')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), '261FA5CE-5DA9-488C-97B1-57E89017621F', 'Which clause is used to filter the results returned by a SELECT query?', 'a) WHERE', 'b) FILTER', 'c) LIMIT', 'd) GROUP', 'Answer: a')

INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), '5910D0A0-6245-4196-ACED-2C6024E198C6', 'What is the correct way to create a function in Python?', 'a) def functionName:', 'b) function functionName():', 'c) def functionName():', 'd) create function functionName()', 'Answer: c')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), '5910D0A0-6245-4196-ACED-2C6024E198C6', 'Which of the following is a mutable data type in Python?', 'a) String', 'b) Tuple', 'c) List', 'd) Integer', 'Answer: c')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), '5910D0A0-6245-4196-ACED-2C6024E198C6', 'What does the len() function do?', 'a) Returns the number of characters in a string', 'b) Returns the length of a list', 'c) Returns the number of elements in a list', 'd) All of the above', 'Answer: d')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), '5910D0A0-6245-4196-ACED-2C6024E198C6', 'What is the output of 3 + 2 * 2?', 'a) 7', 'b) 10', 'c) 9', 'd) 8', 'Answer: a')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), '5910D0A0-6245-4196-ACED-2C6024E198C6', 'Which of the following is used to start a loop in Python?', 'a) repeat', 'b) for', 'c) loop', 'd) iterate', 'Answer: b')

INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), 'ED98F1CA-1AC1-415A-8CA4-C5CBB0CEECD8', 'What does CSS stand for?', 'a) Cascading Style Sheets', 'b) Computer Style Sheets', 'c) Creative Style Sheets', 'd) Colorful Style Sheets', 'Answer: a')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), 'ED98F1CA-1AC1-415A-8CA4-C5CBB0CEECD8', 'Which of the following is the correct syntax to apply a background color in CSS?', 'a) background-color: blue', 'b) bg-color: blue', 'c) color-background: blue', 'd) color-bg: blue', 'Answer: a')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), 'ED98F1CA-1AC1-415A-8CA4-C5CBB0CEECD8', 'How do you add an external CSS file to an HTML document?', 'a) <link rel="stylesheet" type="text/css" href="styles.css">', 'b) <style src="styles.css"></style>', 'c) <stylesheet link="styles.css"></stylesheet>', 'd) <link src="styles.css" type="text/css">', 'Answer: a')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), 'ED98F1CA-1AC1-415A-8CA4-C5CBB0CEECD8', 'Which of the following selectors is used to target an element with a specific ID in CSS?', 'a) .myElement', 'b) #myElement', 'c) *myElement', 'd) &myElement', 'Answer: b')
INSERT INTO [dbo].[Question]([Id],[TopicId],[QuestionText],[Option01],[Option02],[Option03],[Option04],[Answer])VALUES
(newid(), 'ED98F1CA-1AC1-415A-8CA4-C5CBB0CEECD8', 'What is the correct way to make text bold using CSS?', 'a) font-weight: bold', 'b) font-style: bold', 'c) text-weight: bold', 'd) font-size: bold', 'Answer: a');

