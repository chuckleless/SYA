<template style="height:100%">
  <div>
    <el-breadcrumb
      separator-class="el-icon-arrow-right"
      class="recruitment_breadcrumb"
    >
      <el-breadcrumb-item :to="{ path: '/home' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>招聘会</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card
      class="recruitment_card"
      v-loading="loading"
      style="margin-bottom:50px"
    >
      <el-row>
        <el-col :span="7">
          <el-input
            placeholder="请输入内容"
            v-model="queryInfo.query"
            class="input-with-select"
            clearable
            @clear="findWork"
          >
            <el-button
              slot="append"
              icon="el-icon-search"
              @click="findWork"
            ></el-button>
          </el-input>
        </el-col>
        <el-col :span="4"></el-col>
      </el-row>

      <!-- 工作卡片-->
      <el-row>
        <div v-for="work in workList" :key="work.work_id">
          <el-col
            :span="8"
            v-for="(o, index) in 1"
            :key="o"
            :offset="index > 0 ? 1 : 0"
          >
            <el-card :body-style="{ padding: '0px' }" class="recruitment_card2">
              <div style="height:239px;width:400px">
                <img
                  :src="work.cover"
                  class="image"
                  style="height:100%;width:100%"
                />
              </div>
              <div style="padding: 14px;">
                <p>
                  <el-tag type="danger">工作名称</el-tag>
                  : {{ work.work_name }}
                </p>
                <p>
                  <el-tag type="success">点赞</el-tag>
                  : {{ work.likes_num }}
                </p>
                <p>
                  <el-tag type="warning">收藏</el-tag>
                  : {{ work.collect_num }}
                </p>

                <div class="bottom clearfix">
                  <el-button
                    type="primary"
                    class="button"
                    @click="showDrawer(work.work_id)"
                    plain
                    >查看详情</el-button
                  >

                  <el-button
                    type="success"
                    v-if="likestatus[work.work_id] == 1"
                    class="button"
                    @click="GetLike(work.work_id)"
                    plain
                    >取消点赞</el-button
                  >
                  <el-button
                    type="warning"
                    v-if="likestatus[work.work_id] == 0"
                    class="button"
                    @click="GetLike(work.work_id)"
                    plain
                    >点赞</el-button
                  >
                </div>
              </div>
            </el-card>
          </el-col>
        </div>
      </el-row>

      <!-- 右侧弹窗 -->
      <el-drawer
        title="工作详情"
        :visible.sync="drawer"
        :direction="direction"
        :before-close="handleClose"
      >
        <el-card :body-style="{ padding: '10px' }" class="recruitment_card3">
          <div style="height:239px;width:350px">
            <img
              :src="workInfo.cover"
              class="image"
              style="height:100%;width:100%"
            />
          </div>
          <div slot="header" class="clearfix">
            <!--<el-button style="float: right; padding: 3px 0" type="text">操作按钮</el-button>-->
          </div>
          <div>
            <br />
            <el-form :model="workInfo" label-width="75px" class="demo-ruleForm">
              <el-form-item
                label="工作名称:"
                prop="work_name"
                class="demo-ruleFormItem"
              >
                <el-input v-model="workInfo.work_name" readonly></el-input>
              </el-form-item>
              <el-form-item
                label="工作地点:"
                prop="address"
                class="demo-ruleFormItem"
              >
                <el-input v-model="workInfo.address" readonly></el-input>
              </el-form-item>
              <el-form-item
                label="薪资:"
                prop="salary"
                class="demo-ruleFormItem"
              >
                <el-input v-model="workInfo.salary" readonly></el-input>
              </el-form-item>
              <el-form-item
                label="工作日期:"
                prop="start_day"
                class="demo-ruleFormItem"
              >
                <el-input
                  class="work_day"
                  v-model="workInfo.start_day"
                  readonly
                ></el-input>
                <p
                  style="display:inline;width:100px;margin-left:15px;margin-right:15px;"
                >
                  至
                </p>
                <el-input
                  class="work_day"
                  v-model="workInfo.end_day"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item
                label="工作时间:"
                prop="start_time"
                class="demo-ruleFormItem"
              >
                <el-input
                  class="work_day"
                  v-model="workInfo.start_time"
                  readonly
                ></el-input>
                <p
                  style="display:inline;width:100px;margin-left:15px;margin-right:15px;"
                >
                  至
                </p>
                <el-input
                  class="work_day"
                  v-model="workInfo.end_time"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item
                label="工作日:"
                prop="start_time"
                class="demo-ruleFormItem"
                style="width:50%"
              >
                <el-input
                  class="work_day"
                  v-model="weeklist[workInfo.week_day]"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item
                label="工作描述:"
                prop="work_description"
                class="demo-ruleFormItem"
              >
                <el-input
                  type="textarea"
                  :rows="4"
                  v-model="workInfo.work_description"
                  readonly
                ></el-input>
              </el-form-item>
            </el-form>
          </div>
          <div class="demo-drawer__footer">
          <el-button
            type="warning"
            @click="appWork()"
            icon="el-icon-user-solid"
            style="margin-top: 16px;"
            v-if="this.$store.state.role == 1"
            plain
            >提交简历</el-button
          >
          <el-button
            type="primary"
            @click="showFav()"
            icon="el-icon-star-on"
            style="margin-top: 16px;"
            v-if="this.$store.state.role == 1"
            plain
            >加入收藏</el-button
          >
        </div>
        </el-card>
        <!--<div class="demo-drawer__footer">
                        
                    <el-button type="primary" @click="$refs.drawer.closeDrawer()" :loading="loading" style="margin-top: 16px;">{{ loading ? '提交中 ...' : '确 定' }}</el-button>
        </div>-->

        
      </el-drawer>

      <!--  分页区域  -->
      <el-pagination
        @size-change="handleSizeChange3"
        @current-change="handleCurrentChange3"
        :current-page="queryInfo.pagenum"
        :page-sizes="[6, 9, 12]"
        :page-size="6"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      ></el-pagination>
    </el-card>

    <!--查看所有收藏夹-->
    <el-dialog title="详情" :visible.sync="checkDialogVisible" width="50%">
      <el-table :data="favItem" v-loading="loading" class="addfav">
        <el-table-column label="#" type="index"></el-table-column>
        <el-table-column
          label="收藏夹名称"
          prop="favorite_name"
        ></el-table-column>

        <el-table-column
          label="收藏夹工作数量"
          prop="work_num"
        ></el-table-column>

        <el-table-column label="收藏夹ID" prop="favorite_id"></el-table-column>

        <el-table-column label="操作">
          <template v-slot:default="scope">
            <el-tooltip
              effect="dark"
              content="添加收藏夹"
              placement="top-start"
              :enterable="false"
            >
              <el-button
                type="success"
                icon="el-icon-star-on"
                size="mini"
                @click="addWorkFav(scope.row.favorite_id, 0)"
              ></el-button>
            </el-tooltip>
            <el-tooltip
              effect="dark"
              content="查看收藏夹"
              placement="top-start"
              :enterable="false"
            >
              <el-button
                type="danger"
                icon="el-icon-star-off"
                size="mini"
                @click="showFavWork(scope.row.favorite_id)"
              ></el-button>
            </el-tooltip>
          </template>
        </el-table-column>
      </el-table>
      <!--分页区域-->
      <el-pagination
        @size-change="handleSizeChange2"
        @current-change="handleCurrentChange2"
        :current-page="queryInfo2.pagenum"
        :page-sizes="[3, 6, 9]"
        :page-size="3"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total2"
      ></el-pagination>
    </el-dialog>

    <!--左侧弹窗-->
    <el-drawer
      title="收藏详情"
      :visible.sync="drawer2"
      :direction="direction2"
      :before-close="handleClose2"
    >
      <el-card :body-style="{ padding: '10px' }" class="recruitment_card4">
        <el-table :data="favWorkList" v-loading="loading" class="addfav">
          <el-table-column label="#" type="index"></el-table-column>
          <el-table-column label="工作名称" prop="work_name"></el-table-column>

          <el-table-column
            label="工作描述"
            prop="work_description"
          ></el-table-column>

          <el-table-column label="工作ID" prop="work_id"></el-table-column>
        </el-table>
      </el-card>
      <div class="demo-drawer__footer" style="margin-top:15px">
        <el-button
          type="info"
          icon="el-icon-star-on"
          plain
          @click="addWorkFav(fav_id, 1)"
          >加入收藏</el-button
        >
      </div>
    </el-drawer>
  </div>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      input1: "",
      input2: "",
      input3: "",
      select: "",
      loading: true,
      direction: "rtl",
      direction2: "ltr",
      checkDialogVisible: false,

      //获取工作列表的参数对象
      queryInfo: {
        pagenum: 1,
        pagesize: 6,
        query: "",
      },
      queryInfo2: {
        pagenum: 1,
        pagesize: 6,
        query: "",
      },
      workList: [],
      weeklist: [
        "未设置",
        "星期一",
        "星期二",
        "星期三",
        "星期四",
        "星期五",
        "星期六",
        "星期日",
      ],

      favItem: [],
      total: 0,
      total2: 0,
      drawer: false,
      drawer2: false,
      workInfo: {},
      favInfo: {},
      fav_id: -1,
      favWorkList: [],
      likestatus: [],
    };
  },
  async created() {
    //调用获取发布的工作的API函数
    //  this.getWorkInfo()
    await this.getWorkList();
  },
  methods: {
    //监听每页条数选项改变的事件
    handleSizeChange(newSize) {
      this.loading = true;
      //console.log(newSize)
      this.queryInfo.pagesize = newSize;
      this.getWorkList();
      this.loading = false;
    },
    //监听页码改变的事件
    handleCurrentChange(newPage) {
      this.loading = true;
      //console.log(newPage)
      this.queryInfo.pagenum = newPage;
      this.getWorkList();
      this.loading = false;
    },

    handleSizeChange2(newSize) {
      this.loading = true;
      //console.log(newSize)
      this.queryInfo2.pagesize = newSize;
      this.showFav();
      this.loading = false;
    },
    //监听页码改变的事件
    handleCurrentChange2(newPage) {
      this.loading = true;
      //console.log(newPage)
      this.queryInfo2.pagenum = newPage;
      this.showFav();
      this.loading = false;
    },
    async handleSizeChange3(newSize) {
      this.loading = true;
      //console.log(newSize)
      this.queryInfo.pagesize = newSize;
      await this.findWork();
      this.loading = false;
    },
    //监听页码改变的事件
    async handleCurrentChange3(newPage) {
      this.loading = true;
      //console.log(newPage)
      this.queryInfo.pagenum = newPage;
      await this.findWork();
      this.loading = false;
    },

    handleClose(done) {
      this.$confirm("确认关闭？")
        .then((_) => {
          done();
        })
        .catch((_) => {});
    },
    handleClose2(done) {
      this.$confirm("确认关闭？")
        .then((_) => {
          done();
        })
        .catch((_) => {});
    },

    async showLike(workid) {
      const res = await axios.post(
        this.$helper.endpointUrl("/Work/ShowLike"),
        {
          work_id: workid,
        },
        { withCredentials: true }
      );
      if (res.status !== 200) {
        this.$message.error("Unexpected response");

        return 0;
      }
      return res.data;
    },

    //展示右侧弹窗
    async showDrawer(workid) {
      this.direction = "rtl";
      //console.log(workid)
      const res = await axios.post(
        this.$helper.endpointUrl("/Work/ViewWorkInfo"),
        { work_id: workid },
        { withCredentials: true }
      );
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }

      this.workInfo = res.data;

      //console.log(res);
      this.drawer = true;
    },
    //获取工作列表的函数
    async getWorkList() {
      const res = await axios.post(
        this.$helper.endpointUrl("/Work/ViewAllWork"),
        {
          pagenum: this.queryInfo.pagenum,
          pagesize: this.queryInfo.pagesize,
        },
        {
          withCredentials: true,
        }
      );
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      this.workList = res.data.worklist;
      for (var i = 0; i < this.workList.length; i++) {
        let a = await this.showLike(this.workList[i].work_id);
        //console.log("???", i, a);

        this.likestatus[this.workList[i].work_id] = a;
      }
      this.total = res.data.totalpage;
      this.pagesize = res.data.totalpage / res.data.pagenum;
      this.pagenum = res.data.pagenum;
      this.loading = false;
    },

    async findWork() {
      const res = await axios.post(
        this.$helper.endpointUrl("/Work/FindWork"),
        {
          pagenum: this.queryInfo.pagenum,
          pagesize: this.queryInfo.pagesize,
          query: this.queryInfo.query,
        },
        { withCredentials: true }
      );
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      this.workList = res.data.worklist;
      for (var i = 0; i < this.workList.length; i++) {
        let a = await this.showLike(this.workList[i].work_id);

        this.likestatus[this.workList[i].work_id] = a;
      }
      this.total = res.data.totalpage;

      this.pagenum = res.data.pagenum;
    },

    async GetLike(workid) {
      const res = await axios.post(
        this.$helper.endpointUrl("/Work/GetLike"),
        {
          work_id: workid,
        },
        { withCredentials: true }
      );
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      if (res.data == 0) {
        //console.log('su!');
        //this.$message.warning("您已经取消点赞");
        this.$message({
          message: "您已经取消点赞",
          type: "warning",
          duration: 1000,
        });
        this.likestatus[workid] = 0;
        this.findWork();
      } else if (res.data == 1) {
        // this.$message.success("感谢您的点赞");
        this.$message({
          message: "感谢您的点赞",
          type: "success",
          duration: 1000,
        });
        this.likestatus[workid] = 1;
        this.findWork();
      }
    },
    async appWork() {
      const res = await axios.post(
        this.$helper.endpointUrl("/Apply/CreateApply"),
        { work_id: this.workInfo.work_id },
        {
          withCredentials: true,
        }
      );
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      if (res.data == -10) {
        //this.$message.warning("请先创建简历");
        this.$message({
          message: "请先创建简历",
          type: "warning",
          duration: 1000,
        });
        return;
      } else if (res.data == -21) {
        this.$message({
          message: "您已经获得该工作，请到工作管理中查看",
          type: "warning",
          duration: 1000,
        });
        return;
      } else if (res.data == -22) {
        this.$message({
          message: "您已经申请过该工作并正在审核，请耐心等待",
          type: "warning",
          duration: 1000,
        });
        return;
      }
      // this.$message.success("申请此工作成功");
      this.$message({
        message: "申请此工作成功",
        type: "success",
        duration: 1000,
      });
    },
    async showFav() {
      const res = await axios.post(
        this.$helper.endpointUrl("/Favorite/GetFavorite"),
        {
          pagenum: this.queryInfo2.pagenum,
          pagesize: this.queryInfo2.pagesize,
        },
        {
          withCredentials: true,
        }
      );
      this.checkDialogVisible = true;
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      this.favItem = res.data.favoriteItem;
      this.total2 = res.data.totalpage;
      //this.pagesize=res.data.totalpage/res.data.pagenum;
      //this.pagenum=res.data.pagenum;
      //console.log(res);
      this.loading = false;
      //  this.$message.success('申请此工作成功');
    },
    async addWorkFav(favoritee_id, flag) {
      const res = await axios.post(
        this.$helper.endpointUrl("/Favorite/AddFavoriteWork"),
        {
          favorite_id: favoritee_id,
          work_id: this.workInfo.work_id,
        },
        {
          withCredentials: true,
        }
      );
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      if (res.data !== 0) {
        // this.$message.error("该工作已在收藏夹内，无法添加");
        this.$message({
          message: "该工作已在收藏夹内，无法添加",
          type: "error",
          duration: 1000,
        });
      } else {
        //this.$message.success("加入收藏夹成功");
        this.$message({
          message: "加入收藏夹成功",
          type: "success",
          duration: 1000,
        });

        this.showFav();
        this.getWorkList();
        //console.log(this.pagesize);
        if (flag === 1) {
          this.showFavWork(favoritee_id);
        }
      }

      //console.log(res);
    },
    //展示左侧弹窗
    async showFavWork(favoritee_id) {
      this.direction2 = "ltr";
      //console.log(this.workInfo.work_id);
      const res = await axios.post(
        this.$helper.endpointUrl("/Favorite/GetFavoriteInfo"),
        {
          favorite_id: favoritee_id,
        },
        {
          withCredentials: true,
        }
      );
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      this.favWorkList = res.data.worklist;
      this.fav_id = favoritee_id;
      this.loading = false;
      //console.log(res);
      this.drawer2 = true;
    },
  },
};
</script>

<style scoped>
.recruitment_breadcrumb {
  position: relative;
  left: 20px;
  margin-bottom: 15px;
  margin-top: 20px;
  width: 90% !important;
}

.recruitment_card {
  box-shadow: 0 10px 10px rgba(0, 0, 0, 0.15) !important;
  position: relative;
  left: 6px;
  width: 99% !important;
  height: 90% !important;
}

.recruitment_card2 {
  box-shadow: 0 10px 10px rgba(0, 0.25, 0, 0.25) !important;
  /*margin-left:70px;
  margin-top:30px;*/
  width: 400px !important;
  margin: auto;
  margin-top: 30px;
  background-color: #fff;
}

.recruitment_card3 {
  box-shadow: 0 10px 10px rgba(0, 0.25, 0, 0.25) !important;
  margin: auto;
  width: 390px !important;
  height: 72% !important;
  overflow: auto;
  background-color: #fff;
}

.recruitment_card4 {
  box-shadow: 0 10px 10px rgba(0, 0.25, 0, 0.25) !important;
  margin: auto;
  width: 390px !important;
  height: 90.6% !important;
  overflow: auto;
  background-color: #fff;
}

.el-select .el-input {
  width: 130px;
}
.input-with-select .el-input-group__prepend {
  background-color: #fff;
}

.el-pagination {
  margin-top: 15px;
}

.time {
  font-size: 13px;
  color: #999;
}

.bottom {
  margin-top: 13px;
  line-height: 8px;
}

.button {
  padding: 0;
  float: center;
  width: 100px;
  height: 40px;
}

.image {
  width: 100%;
  display: block;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}

.clearfix:after {
  clear: both;
}

.demo-ruleFormItem {
  margin-left: -10px;
}

.work_day {
  width: 114px;
}
</style>
