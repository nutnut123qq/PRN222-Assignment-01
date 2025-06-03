-- FU News Management System - Seed Data Script
USE FUNewsManagement;

-- Insert Categories
INSERT INTO Categories (CategoryName, CategoryDescription, ParentCategoryId, IsActive) VALUES
('Technology', 'Technology news and updates', NULL, 1),
('Education', 'Educational news and announcements', NULL, 1),
('Sports', 'Sports news and events', NULL, 1),
('Entertainment', 'Entertainment and cultural news', NULL, 1);

-- Insert System Accounts
INSERT INTO SystemAccounts (AccountName, AccountEmail, AccountRole, AccountPassword) VALUES
('John Staff', 'staff@funews.com', 1, 'staff123'),
('Jane Lecturer', 'lecturer@funews.com', 2, 'lecturer123'),
('Admin User', 'admin@test.com', 1, 'admin123');

-- Insert Tags
INSERT INTO Tags (TagName, Note) VALUES
('Breaking', 'Breaking news tag'),
('Important', 'Important announcements'),
('Event', 'Event related news'),
('Update', 'Updates and changes');

-- Insert News Articles
INSERT INTO NewsArticles (NewsArticleId, NewsTitle, Headline, NewsContent, NewsSource, CategoryId, NewsStatus, CreatedById, CreatedDate) VALUES
('NEWS20250603001', 
 'New Learning Management System Launched', 
 'University introduces advanced LMS for better online learning experience',
 'The university has successfully launched a new Learning Management System (LMS) that provides enhanced features for both students and faculty. The system includes improved user interface, better communication tools, and advanced analytics for tracking student progress. This new system will revolutionize how we approach online education and provide a more seamless experience for all users.',
 'University IT Department',
 2, -- Education category
 1, -- Active
 1, -- Created by first staff account
 GETDATE() - 5),

('NEWS20250603002',
 'Campus WiFi Infrastructure Upgrade',
 'High-speed internet connectivity now available across all campus buildings',
 'The university has completed a major upgrade to its WiFi infrastructure, providing faster and more reliable internet access to all students and staff. The new system supports the latest WiFi 6 technology and covers 100% of the campus area. Students can now enjoy seamless connectivity whether they are in classrooms, libraries, dormitories, or outdoor areas.',
 'Campus Network Team',
 1, -- Technology category
 1, -- Active
 1, -- Created by first staff account
 GETDATE() - 3),

('NEWS20250603003',
 'Student Registration System Maintenance',
 'Scheduled maintenance for student registration system this weekend',
 'The student registration system will undergo scheduled maintenance this weekend from Saturday 6 PM to Sunday 6 AM. During this time, students will not be able to access course registration features. All other services will remain available. We apologize for any inconvenience and appreciate your understanding.',
 'Student Services',
 2, -- Education category
 1, -- Active
 1, -- Created by first staff account
 GETDATE() - 1),

('NEWS20250603004',
 'Annual Sports Festival Announcement',
 'Join us for the biggest sports event of the year',
 'The annual university sports festival is scheduled for next month. This year''s event will feature competitions in football, basketball, volleyball, tennis, and swimming. Registration is now open for all students and staff. Prizes will be awarded to winners in each category. Don''t miss this exciting opportunity to showcase your athletic skills!',
 'Sports Department',
 3, -- Sports category
 1, -- Active
 1, -- Created by first staff account
 GETDATE() - 2);

-- Insert News Tags (Many-to-Many relationships)
INSERT INTO NewsTags (NewsArticleId, TagId) VALUES
('NEWS20250603001', 2), -- Important
('NEWS20250603001', 4), -- Update
('NEWS20250603002', 1), -- Breaking
('NEWS20250603002', 4), -- Update
('NEWS20250603003', 2), -- Important
('NEWS20250603004', 3); -- Event

-- Verify data insertion
SELECT 'Categories' as TableName, COUNT(*) as RecordCount FROM Categories
UNION ALL
SELECT 'SystemAccounts', COUNT(*) FROM SystemAccounts
UNION ALL
SELECT 'Tags', COUNT(*) FROM Tags
UNION ALL
SELECT 'NewsArticles', COUNT(*) FROM NewsArticles
UNION ALL
SELECT 'NewsTags', COUNT(*) FROM NewsTags;

-- Display sample data
SELECT 'Sample News Articles:' as Info;
SELECT 
    na.NewsArticleId,
    na.NewsTitle,
    c.CategoryName,
    sa.AccountName as CreatedBy,
    na.CreatedDate,
    CASE WHEN na.NewsStatus = 1 THEN 'Active' ELSE 'Inactive' END as Status
FROM NewsArticles na
JOIN Categories c ON na.CategoryId = c.CategoryId
JOIN SystemAccounts sa ON na.CreatedById = sa.AccountId
ORDER BY na.CreatedDate DESC;
