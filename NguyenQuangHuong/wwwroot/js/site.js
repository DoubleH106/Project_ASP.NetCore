function showUserList() {
    var userList = document.getElementById('userList');
    if (userList.style.display === 'none') {
        userList.style.display = 'block';
    } else {
        userList.style.display = 'none';
    }
}