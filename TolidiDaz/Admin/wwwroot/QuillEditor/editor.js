window.QuillFunctions = {
    createQuill: function (quillElement, dotNetRef) {
        var quill = new Quill(quillElement, {
            theme: 'snow',
            placeholder: 'متن خود را اینجا بنویسید...',
            modules: {
                toolbar: [
                    [{ 'font': [] }],
                    [{ 'size': ['small', false, 'large', 'huge'] }],
                    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                    ['bold', 'italic', 'underline', 'strike'],
                    [{ 'color': [] }, { 'background': [] }],
                    [{ 'script': 'sub' }, { 'script': 'super' }],
                    ['blockquote', 'code-block'],
                    [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'list': 'check' }],
                    [{ 'indent': '-1' }, { 'indent': '+1' }],
                    [{ 'direction': 'rtl' }], // دکمه تغییر جهت
                    [{ 'align': [] }],        // تراز متن
                    ['link', 'image', 'video'],
                    ['clean']
                ]
            }
        });

        // --- تنظیمات پیش‌فرض راست‌چین (RTL) ---
        // 1. تنظیم جهت محتوا به RTL
        quill.format('direction', 'rtl');
        // 2. تنظیم تراز متن به راست
        quill.format('align', 'right');
        // 3. تنظیم ویژگی HTML برای نمایش درست نشانگر (Cursor)
        quill.root.setAttribute('dir', 'rtl');
        quill.root.style.textAlign = 'right';

        // آپدیت کردن مقدار در Blazor هنگام تغییر متن
        quill.on('text-change', function () {
            var html = quill.root.innerHTML;
            dotNetRef.invokeMethodAsync('UpdateHtml', html);
        });

        return quill;
    },

    getHtml: function (quill) {
        return quill.root.innerHTML;
    },

    setHtml: function (quill, html) {
        quill.root.innerHTML = html;
    }
};
