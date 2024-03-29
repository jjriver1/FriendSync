<template>
  <v-card max-width="400px">
    <v-card-title>
      <div style="text-align: center">
        <h1 class="mb-2">Create Account</h1>
      </div>
    </v-card-title>
    <v-card-text>
      <v-row>
        <v-form>
          <!-- should the row be nested inside the form? -->
          <v-col cols="12">
            <v-text-field
              v-model="username"
              label="Username"
              outlined
              required
              @input="validateUsername"
            ></v-text-field>
          </v-col>
          <v-col cols="12">
            <v-text-field
              v-model="email"
              label="Email"
              outlined
              required
              @input="validateEmail"
            ></v-text-field>
          </v-col>
          <v-col cols="12">
            <v-text-field
              v-model="password"
              label="Password"
              type="password"
              outlined
              required
              @input="validatePassword"
            ></v-text-field>
          </v-col>
          <v-col cols="12">
            <v-text-field
              v-model="confirmPassword"
              label="Confirm Password"
              type="password"
              outlined
              required
              @input="validateConfirmPassword"
            ></v-text-field>
          </v-col>
          <v-col cols="12">
            <p>{{ statusText }}</p>
          </v-col>
          <v-col cols="12">
            <v-btn :disabled="!validateForm" @click="createAccount">
              Create Account
            </v-btn>
            <v-btn @click="resetForm">Reset</v-btn>
          </v-col>
        </v-form>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import createAccount from "@/api.ts";
import { ResponseCodes } from "@/enums/ResponseCodes.ts";

export default defineComponent({
  name: "CreateAccountCard",
  data() {
    return {
      username: "",
      email: "",
      password: "",
      confirmPassword: "",
      validUsername: false,
      validEmail: false,
      validPassword: false,
      validConfirmPassword: false,
      statusText: "",
    };
  },
  computed: {
    validateForm() {
      return (
        this.validUsername &&
        this.validEmail &&
        this.validPassword &&
        this.validConfirmPassword
      );
    },
  },
  methods: {
    validateUsername() {
      this.validUsername = this.username.length > 0;
    },
    validateEmail() {
      this.validEmail = this.email.includes("@");
    },
    validatePassword() {
      this.validPassword = this.password.length > 6;
    },
    validateConfirmPassword() {
      this.validConfirmPassword = this.password === this.confirmPassword;
    },
    async createAccount() {
      const response = await createAccount({
        id: "", // should be generated by the server
        username: this.username,
        email: this.email,
        password: this.password,
        bio: "",
      });

      // noinspection JSIncompatibleTypesComparison
      if (response === null || response === undefined) {
        console.error("Failed to create account. Response was null.");
        this.statusText = "Failed to create account. Response was null.";
        return;
      }

      if (response.status !== ResponseCodes.CREATED) {
        console.error(
          "Failed to create account. \n" +
            "Status was: " +
            response.status +
            "\nData was: " +
            response.data,
        );

        this.statusText = response.data;
        return;
      }

      if (response.status === ResponseCodes.CREATED) {
        //this.$router.push("/login");
        console.log("Account created");
        this.statusText = "Account created";
      }

      this.resetForm();
    },
    resetForm() {
      this.email = "";
      this.password = "";
      this.confirmPassword = "";
      this.validEmail = false;
      this.validPassword = false;
      this.validConfirmPassword = false;
    },
  },
});
</script>

<style scoped lang="sass">

</style>
