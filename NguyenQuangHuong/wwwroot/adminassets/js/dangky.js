namespace Test_Project3.wwwroot.adminassets.js {
    document.getElementById("loginForm").addEventListener("submit", function (event) {
        event.preventDefault(); // Prevent form submission

        // Get input values
        var username = document.getElementById("username").value;
        var password = document.getElementById("password").value;

        // Perform login validation
        if (username === "admin" && password === "password") {
            alert("Đăng nhập thành công!");
            // Redirect to another page
            window.location.href = "dashboard.html";
        } else {
            alert("Đăng nhập không thành công. Vui lòng kiểm tra tên người dùng và mật khẩu.");
        }
    });
}
