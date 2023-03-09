<template>
  <div class="container">
    <img src="../assets/bike.png" height="100" />
    <div class="heading">Logout</div>
    <div class="status">{{ status }}</div>
  </div>
</template>
<script>
import { ref, onMounted } from "vue";
export default {
  setup() {
    let status = ref("You are Logged out.");
    onMounted(() => {
      logout();
    });
    const logout = () => {
      sessionStorage.clear();
    };
    return { status };
  },
};
</script>
