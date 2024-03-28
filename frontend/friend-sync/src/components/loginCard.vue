<template>
  <v-card title="Login" width="400" class="text-center pa-4" color="secondary">
    <v-sheet class="mx-auto" width="300" color="secondary">
      <v-form>
        <v-text-field label="Username"></v-text-field>
        <v-text-field label="Password"></v-text-field>
        <!--<div>Forgot Password?</div>-->
        <v-btn class="mt-2" type="submit" block>Log In</v-btn>
        <v-divider class="ma-4 border-opacity-50"></v-divider>
        <v-btn class="mt-2" type="submit" block>Create New Account</v-btn>
      </v-form>
    </v-sheet>
  </v-card>
</template>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "LoginCard",
});
</script>

<style scoped>
</style>
