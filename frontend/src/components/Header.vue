<template>
  <div>
    <el-row>
      <el-col :span="4" style="padding-top:12px;padding-right:12px">
        <!-- <div> -->
        <!-- <el-container> -->
        <!-- <div class="logo"> -->
        <div>
          <img
            src="../assets/imagebox/sya_logo.png"
            alt
            height="38"
            style="float: left;padding-left:50px"
            @click="toHome()"
            class="img"
          />
        </div>
        <!-- </div> -->
        <!-- </el-container> -->
        <!-- </div> -->
      </el-col>
      <el-col :span="16">
        <div style="display: inline-block">
          <el-menu
            class="el-menu-demo"
            mode="horizontal"
            background-color="rgba(220,38,38,0)"
            text-color="#fff"
            active-text-color="#ffd04b"
            :default-active="getDefaultActive()"
            router
          >
            <el-menu-item index="/Home">首页</el-menu-item>
            <el-menu-item index="/Recruitment">招聘会</el-menu-item>
            <el-menu-item index="/TeacherTable" v-if="this.$store.state.role==2">工作台</el-menu-item>
            <!-- <el-menu-item index="/AdminTable" v-if="this.$store.state.role==0">管理员工作台</el-menu-item> -->
            <el-menu-item index="/PublishSystemMessage" v-if="this.$store.state.role==0">工作台</el-menu-item>
            <el-menu-item index="/StudentMessage" v-if="this.$store.state.role==1">消息</el-menu-item>
            <el-menu-item index="/TeacherMessage" v-if="this.$store.state.role==2">消息</el-menu-item>
            <!--  <el-menu-item index="/AdminMessage" v-if="this.$store.state.role==2">管理员消息</el-menu-item>-->
            <!-- <el-menu-item index="/AdminMine" v-if="this.$store.state.role==0">我的</el-menu-item> -->
            <el-menu-item index="/StudentFavorites" v-if="this.$store.state.role==1">收藏夹</el-menu-item>
            <!-- <el-menu-item index="/TeacherFavorites" v-if="this.$store.state.role==2">老师收藏夹</el-menu-item> -->
            <el-menu-item index="/StudentMine" v-if="this.$store.state.role==1">我的</el-menu-item>
            <el-menu-item index="/TeacherMine" v-if="this.$store.state.role==2">我的</el-menu-item>
          </el-menu>
        </div>
      </el-col>
      <el-col :span="4">
        <div
          height="38px"
          style="padding-top:10px;float: right;padding-right:30px"
          class="img"
          v-if="!this.$store.state.role==0"
        >
          <el-dropdown>
            <el-avatar shape="circle">
              <el-image :src="imgSrc" fit="fill" style="height:40px;width:40px"></el-image>
            </el-avatar>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item>
                <div @click="toInfo()">我的资料</div>
              </el-dropdown-item>
              <el-dropdown-item>
                <div @click="logOut()">退出登录</div>
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </div>
        <div height="38px" style="padding-top:10px;float: right;padding-right:30px">
          <el-button type="primary" v-if="this.$store.state.role==0" @click="logOut()">退出登录</el-button>
        </div>
      </el-col>
    </el-row>
  </div>
</template>


<script>
import axios from "axios";
export default {
  data() {
    return {
      imgSrc: ""
    };
  },
  methods: {
    getDefaultActive() {
      //console.log(this.$route.path);
      // if (this.$route.path == "/TeacherInformation") return "/TeacherMine";
      // else if (this.$route.path == "/StudentInformation") return "/StudentMine";
      return;
    },
    toHome() {
      if (("route", this.$route.path === "/Home")) return;
      this.$router.push("/Home");
    },
    async toInfo() {
      if (this.$store.state.role == 1) {
        this.$router.push("/StudentInformation");
      } else if (this.$store.state.role == 2) {
        this.$router.push("/TeacherInformation");
      }
    },
    async logOut() {
      const res = await axios.post(
        this.$helper.endpointUrl("/Account/Logout"),
        {},
        { withCredentials: true }
      );
      //console.log(res);
      if (res.status == 204) {
        //将登录信息保存到vuex
        this.$store.commit("logOut");
        //将vuex里的信息保存到sessionStorage里
        sessionStorage.setItem("store", {});
        this.$message({
          message: "您已退出成功",
          type: "success",
          duration: 1000,
        });
        this.$router.push("/LogIn");
      }
    }
  },
  async created() {
    const { data: res } = await axios.post(
      this.$helper.endpointUrl("/User/GetUserInfo"),
      {},
      { withCredentials: true }
    );
    // console.log("header", res.avatar);
    // console.log(res);
    if (!res.avatar) {
      this.imgSrc = this.$helper.defaultAvatar;
    } else {
      this.imgSrc = res.avatar;
      // console.log("img", typeof res.avatar, res.avatar);
    }
  }
};
</script>

<style scoped>
.el-menu-item {
  background-color: rgba(220, 38, 38, 0) !important;
}
.img {
  cursor: pointer;
}
</style>
