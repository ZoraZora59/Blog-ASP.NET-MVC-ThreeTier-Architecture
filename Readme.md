# 博客项目 - MVC+三层架构重构版

----

## 项目已完毕

#### 参考资料：http://www.cnblogs.com/selimsong/p/7641799.html

## 项目初始化方式：

    1. 通过NuGet包管理器-管理解决方案的NuGet程序包，还原解决方案的NUGET程序包。
    2. 设置BlogRefactored为默认启动项目。
    3. 在BlogRefactored项目的APP_DATA目录中新建数据库，命名为"NewBeeBlog.mdf"。
    4. 重新生成解决方案，并运行解决方案，即BlogRefactored项目。
    5. 浏览器中首页加载完毕即可关闭。
    6. 在VisualStudio中进入"服务器资源管理器"，对"NewBeeBlog.mdf"进行修改。
    7. 将BlogTexts表中，'Hot','PreID','NexID'三项的默认值设为'0','TextChangeDate'的默认值设为'GETDATE()'。
    8. 将BlogComments表中，'CommentChangeDate'的默认值设为'GETDATE()'。

        备注：
            项目默认数据库使用LocalDB，请确保环境是否已安装。
            如需修改数据库，需在BlogRefactored和BlogDAL两个项目的Web.config中修改连接字符串。
