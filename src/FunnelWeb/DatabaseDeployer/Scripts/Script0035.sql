/* Hide comments on the recent post page. */
INSERT INTO $schema$.[Setting]([Name], [Description], [DisplayName], [Value])
VALUES ('spam-comment-hide-count', 'Hide comment count on the recent posts page.', 'Hide comment counts', 'false')
