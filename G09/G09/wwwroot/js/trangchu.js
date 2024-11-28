let love_icons = document.querySelectorAll(".like");
love_icons.forEach(function (icon) {
    icon.addEventListener("click", function () {
        let not_loved = icon.querySelector(".not_loved");
        let loved = icon.querySelector(".loved");

        if (not_loved.classList.contains("hide_img")) {
            // Nếu "not_loved" đang ẩn, hiện nó và ẩn "loved"
            not_loved.classList.remove("hide_img");
            not_loved.classList.add("display");

            loved.classList.remove("display");
            loved.classList.add("hide_img");
        } else {
            // Nếu "not_loved" đang hiện, ẩn nó và hiện "loved"
            not_loved.classList.remove("display");
            not_loved.classList.add("hide_img");

            loved.classList.remove("hide_img");
            loved.classList.add("display");
        }
    });
});

$(document).ready(function () {

    $('.button_like').click(function () {
        var mabaiviet = $(this).data('mabaiviet');
        var tennguoidung = $(this).data('tennguoidung');
        // alert(mabaiviet+"_"+tennguoidung)


        $.post('/TrangChu/LikeEvent', { mabaiviet: mabaiviet, tennguoidung: tennguoidung }, function (response) {
            if (response.success) {
                //alert('Done');
                $('#like-count-' + mabaiviet).text(response.newLikeCount + ' likes');
            } else {
                if (response.message == "F") {
                    alert('Fail');

                }

            }
        });
    });


    $('.comment-section').on('keypress', '.commentInput', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault(); // Ngăn chặn hành động mặc định của Enter trong form

            var commentInput = $(this);
            var comment = commentInput.val();
            var parentDiv = commentInput.closest('.comment-section');
            var mabaiviet = parentDiv.data('mabaiviet');
            var tennguoidung = parentDiv.data('tennguoidung');
            //alert(comment + "_" + mabaiviet + "_" + tennguoidung);
            $.post('/TrangChu/AddComment', { comment: comment, mabaiviet: mabaiviet, tennguoidung: tennguoidung }, function (response) {
                if (response.success) {
                    // alert('Comment added');
                    commentInput.val(''); // Xóa nội dung trường nhập

                    // Thêm bình luận mới vào giao diện
                    var newComment = '<p><a class="bold" href="#">' + response.tennguoidung + '</a> ' + comment + '</p>';
                    parentDiv.before(newComment); // Thêm bình luận mới vào trước khối comment-section
                } else {
                    alert('Failed to add comment');
                }
            });
        }
    });

});